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
        private BaseHandler<CallDataRecordContext> extractor;

        /// <summary>
        /// The parser
        /// </summary>
        private BaseHandler<CallDataRecordContext> parser;

        /// <summary>
        /// The aggregator
        /// </summary>
        private BaseHandler<CallDataRecordContext> aggregator;

        private Cleaner cleaner;

        private CallDataRecordContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Delegate for progress event
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="progressValue">The progress value.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public delegate void ProgressHanlder(string stepName, int progressValue, EventArgs e);

        /// <summary>
        /// Handles the Click event of the ProcessButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ProcessButton_Click(object sender, EventArgs e)
        {
            StartProcess("IMEI");
        }

        private void StartProcess(string groupByField)
        {
            this.context = new CallDataRecordContext(txtInputFolder.Text, txtExtractor.Text)
            {
                OutputPath = txtOutputFolder.Text,
                GroupByField = groupByField
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
            this.startOfChain = new Initializer();
            this.extractor = new Extractor();
            this.parser = new Parser();
            this.aggregator = new Aggregator();
            this.cleaner = new Cleaner();

            this.startOfChain.SetSuccessor(this.extractor);
            
            this.extractor.SetSuccessor(this.parser);
            this.parser.SetSuccessor(this.aggregator);
            this.aggregator.SetSuccessor(this.cleaner);
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

        private void Context_ProcessProgressChanged(int progress)
        {
            this.progressUpdateBGWorker.ReportProgress(progress);
        }

        private void ProgressUpdateBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
          
            context.ProcessProgressChanged += Context_ProcessProgressChanged;
            context.ProcessStepChanged += Context_ProcessStepChanged;

            this.startOfChain = CreateChain();
            this.startOfChain.Process(this.context);
        }

        private void Context_ProcessStepChanged(string step, bool abort)
        {
            this.BeginInvoke((Action)(() => this.StepLabel.Text = step));
            if (step == "Done!" || abort)
            {
                InputPanel.SetPropertyInGuiThread(p => p.Enabled, true);
            }
        }

        private void progressUpdateBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /* display */
            this.overAllProgressBar.Value = e.ProgressPercentage;
            this.lblPercentage.Text = e.ProgressPercentage + "%";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

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

        private void SummarizeBySIMButton_Click(object sender, EventArgs e)
        {
            StartProcess("PhoneNumber");
        }
    }
}
