// -----------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Global Supply Chain Services (Ltd)">
//     Copyright (c) GlobalTrack. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MTNDataAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Windows.Threading;
    using MTNDataAnalysis.Chain;
    using MTNDataAnalysis.Context;
    using MTNDataAnalysis.Helpers;
    using System.IO;

    /// <summary>
    /// Main User Interface
    /// </summary>
    public partial class MainForm : Form
    {
        private static System.Timers.Timer ProgressTimer; 
        /// <summary>
        /// The start of chain
        /// </summary>
        private BaseHandler<CallDataRecordContext> startOfChain;

        /// <summary>
        /// The extractor
        /// </summary>
        private BaseHandler<CallDataRecordContext> extractStep;

        /// <summary>
        /// The parser
        /// </summary>
        private BaseHandler<CallDataRecordContext> parseStep;

        /// <summary>
        /// The aggregator
        /// </summary>
        private BaseHandler<CallDataRecordContext> summarizeStep;

        /// <summary>
        /// The cleaner
        /// </summary>
        private CleanupStep cleanupStep;

        /// <summary>
        /// The context
        /// </summary>
        private CallDataRecordContext context;

        /// <summary>
        /// The write to file step
        /// </summary>
        private WriteToFilesStep writeToFileStep;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the ProcessButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ProcessButton_Click(object sender, EventArgs e)
        {
            StartProcess("IMEI");
        }

        /// <summary>
        /// Starts the process.
        /// </summary>
        /// <param name="groupByField">The group by field.</param>
        private void StartProcess(string groupByField)
        {
            this.context = new CallDataRecordContext(txtInputFolder.Text, txtExtractor.Text)
            {
                OutputPath = txtOutputFolder.Text,
                GroupByField = groupByField,
                StartTime = DateTime.Now
            };

            this.InputPanel.Enabled = false;
            progressUpdateBGWorker.WorkerReportsProgress = true;
            progressUpdateBGWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Creates the chain.
        /// </summary>
        /// <returns>
        /// the initial step of the chain
        /// </returns>
        private BaseHandler<CallDataRecordContext> CreateChain()
        {
            this.startOfChain = new InitializeStep();
            this.extractStep = new ExtractArchivesStep();
            this.parseStep = new ParseStep();
            this.summarizeStep = new SummarizeStep();
            this.writeToFileStep = new WriteToFilesStep();
            this.cleanupStep = new CleanupStep();

            this.startOfChain.SetSuccessor(this.extractStep);
            
            this.extractStep.SetSuccessor(this.parseStep);
            this.parseStep.SetSuccessor(this.summarizeStep);
            this.summarizeStep.SetSuccessor(this.writeToFileStep);
            this.writeToFileStep.SetSuccessor(this.cleanupStep);
            return this.startOfChain;
        }

        /// <summary>
        /// Handles the Click event of the BrowseInputFolderButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BrowseInputFolderButton_Click(object sender, EventArgs e)
        {
            folderDialog.SelectedPath = txtInputFolder.Text;
            if (folderDialog.ShowDialog() == DialogResult.OK)
            { 
                txtInputFolder.Text = folderDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Handles the Click event of the BrowseOutputFolderButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BrowseOutputFolderButton_Click(object sender, EventArgs e)
        {
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                txtOutputFolder.Text = folderDialog.SelectedPath;
            }
        }

       
        /// <summary>
        /// Handles the Click event of the Extractor Browse Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BrowseExtractorExeButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog.Multiselect = false;
            OpenFileDialog.Filter = "Execultable files (*.exe)|*.exe";
            OpenFileDialog.FileName = string.Empty;
            if (OpenFileDialog.ShowDialog() == DialogResult.OK) 
            {
                txtExtractor.Text = OpenFileDialog.FileName;
            }
        }

        /// <summary>
        /// Context_s the process progress changed.
        /// </summary>
        /// <param name="progress">The progress.</param>
        private void Context_ProcessProgressChanged(int progress)
        {
            this.progressUpdateBGWorker.ReportProgress(progress);
        }

        /// <summary>
        /// Handles the DoWork event of the ProgressUpdateBGWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void ProgressUpdateBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
          
            context.ProcessProgressChanged += Context_ProcessProgressChanged;
            context.ProcessStepChanged += Context_ProcessStepChanged;

            this.startOfChain = CreateChain();
            this.startOfChain.Process(this.context);
        }

        /// <summary>
        /// Context_s the process step changed.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <param name="abort">if set to <c>true</c> [abort].</param>
        private void Context_ProcessStepChanged(string step, bool abort)
        {
            this.BeginInvoke((Action)(() => this.StepLabel.Text = step));
            if (step == "Done!" || abort)
            {
                InputPanel.SetPropertyInGuiThread(p => p.Enabled, true);
            }
        }

        /// <summary>
        /// Handles the ProgressChanged event of the progressUpdateBGWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
        private void progressUpdateBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /* display */
            this.overAllProgressBar.Value = e.ProgressPercentage;
            this.lblPercentage.Text = e.ProgressPercentage + "%";
        }

        /// <summary>
        /// Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            var defaultDataDirectory = Application.StartupPath + "\\Data";
            txtInputFolder.Text = defaultDataDirectory;
            if (!Directory.Exists(txtInputFolder.Text))
            {
                try
                {
                    Directory.CreateDirectory(defaultDataDirectory);
                }
                catch (IOException iox)
                {
                    MessageBox.Show(iox.Message);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the SummarizeBySIMButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SummarizeBySIMButton_Click(object sender, EventArgs e)
        {
            StartProcess("PhoneNumber");
        }
    }
}
