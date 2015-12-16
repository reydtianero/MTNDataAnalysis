namespace MTNDataAnalysis
{

    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InputPanel = new System.Windows.Forms.Panel();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.txtInputFolder = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnBrowseOutputFolder = new System.Windows.Forms.Button();
            this.btnBrowseInput = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.btnExtractor = new System.Windows.Forms.Button();
            this.txtExtractor = new System.Windows.Forms.TextBox();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.progressUpdateBGWorker = new System.ComponentModel.BackgroundWorker();
            this.lblPercentage = new System.Windows.Forms.Label();
            this.overAllProgressBar = new System.Windows.Forms.ProgressBar();
            this.StepLabel = new System.Windows.Forms.Label();
            this.SummarizeBySIMButton = new System.Windows.Forms.Button();
            this.InputPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // InputPanel
            // 
            this.InputPanel.Controls.Add(this.SummarizeBySIMButton);
            this.InputPanel.Controls.Add(this.txtOutputFolder);
            this.InputPanel.Controls.Add(this.pictureBox2);
            this.InputPanel.Controls.Add(this.txtInputFolder);
            this.InputPanel.Controls.Add(this.pictureBox1);
            this.InputPanel.Controls.Add(this.btnBrowseOutputFolder);
            this.InputPanel.Controls.Add(this.btnBrowseInput);
            this.InputPanel.Controls.Add(this.btnProcess);
            this.InputPanel.Controls.Add(this.btnExtractor);
            this.InputPanel.Controls.Add(this.txtExtractor);
            this.InputPanel.Location = new System.Drawing.Point(5, 8);
            this.InputPanel.Name = "InputPanel";
            this.InputPanel.Size = new System.Drawing.Size(519, 113);
            this.InputPanel.TabIndex = 7;
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(40, 38);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(281, 20);
            this.txtOutputFolder.TabIndex = 8;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::MTNDataAnalysis.Properties.Resources.application_export;
            this.pictureBox2.Location = new System.Drawing.Point(17, 33);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 25);
            this.pictureBox2.TabIndex = 16;
            this.pictureBox2.TabStop = false;
            // 
            // txtInputFolder
            // 
            this.txtInputFolder.Location = new System.Drawing.Point(40, 12);
            this.txtInputFolder.Name = "txtInputFolder";
            this.txtInputFolder.Size = new System.Drawing.Size(281, 20);
            this.txtInputFolder.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox1.Image = global::MTNDataAnalysis.Properties.Resources.application_import;
            this.pictureBox1.Location = new System.Drawing.Point(13, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 28);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // btnBrowseOutputFolder
            // 
            this.btnBrowseOutputFolder.Location = new System.Drawing.Point(327, 37);
            this.btnBrowseOutputFolder.Name = "btnBrowseOutputFolder";
            this.btnBrowseOutputFolder.Size = new System.Drawing.Size(33, 20);
            this.btnBrowseOutputFolder.TabIndex = 13;
            this.btnBrowseOutputFolder.Text = "...";
            this.btnBrowseOutputFolder.UseVisualStyleBackColor = true;
            this.btnBrowseOutputFolder.Click += new System.EventHandler(this.BrowseOutputFolderButton_Click);
            // 
            // btnBrowseInput
            // 
            this.btnBrowseInput.Location = new System.Drawing.Point(327, 12);
            this.btnBrowseInput.Name = "btnBrowseInput";
            this.btnBrowseInput.Size = new System.Drawing.Size(33, 20);
            this.btnBrowseInput.TabIndex = 12;
            this.btnBrowseInput.Text = "...";
            this.btnBrowseInput.UseVisualStyleBackColor = true;
            this.btnBrowseInput.Click += new System.EventHandler(this.BrowseInputFolderButton_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(365, 12);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(68, 46);
            this.btnProcess.TabIndex = 9;
            this.btnProcess.Text = "Summarize by IMEI";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.ProcessButton_Click);
            // 
            // btnExtractor
            // 
            this.btnExtractor.Location = new System.Drawing.Point(301, 76);
            this.btnExtractor.Name = "btnExtractor";
            this.btnExtractor.Size = new System.Drawing.Size(33, 20);
            this.btnExtractor.TabIndex = 26;
            this.btnExtractor.Text = "...";
            this.btnExtractor.UseVisualStyleBackColor = true;
            this.btnExtractor.Visible = false;
            // 
            // txtExtractor
            // 
            this.txtExtractor.Location = new System.Drawing.Point(40, 76);
            this.txtExtractor.Name = "txtExtractor";
            this.txtExtractor.Size = new System.Drawing.Size(257, 20);
            this.txtExtractor.TabIndex = 25;
            this.txtExtractor.Text = "C:\\Program Files\\7-Zip\\7z.exe";
            this.txtExtractor.Visible = false;
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "selectFileDialog";
            // 
            // progressUpdateBGWorker
            // 
            this.progressUpdateBGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ProgressUpdateBGWorker_DoWork);
            this.progressUpdateBGWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ProgressUpdateBGWorker_ProgressChanged);
            // 
            // lblPercentage
            // 
            this.lblPercentage.AutoSize = true;
            this.lblPercentage.Location = new System.Drawing.Point(503, 124);
            this.lblPercentage.Name = "lblPercentage";
            this.lblPercentage.Size = new System.Drawing.Size(21, 13);
            this.lblPercentage.TabIndex = 24;
            this.lblPercentage.Text = "0%";
            // 
            // overAllProgressBar
            // 
            this.overAllProgressBar.Location = new System.Drawing.Point(5, 140);
            this.overAllProgressBar.Name = "overAllProgressBar";
            this.overAllProgressBar.Size = new System.Drawing.Size(519, 23);
            this.overAllProgressBar.TabIndex = 22;
            // 
            // StepLabel
            // 
            this.StepLabel.AutoSize = true;
            this.StepLabel.Location = new System.Drawing.Point(8, 124);
            this.StepLabel.Name = "StepLabel";
            this.StepLabel.Size = new System.Drawing.Size(152, 13);
            this.StepLabel.TabIndex = 23;
            this.StepLabel.Text = "Press Proccess when Ready...";
            // 
            // SummarizeBySIMButton
            // 
            this.SummarizeBySIMButton.Location = new System.Drawing.Point(439, 12);
            this.SummarizeBySIMButton.Name = "SummarizeBySIMButton";
            this.SummarizeBySIMButton.Size = new System.Drawing.Size(68, 46);
            this.SummarizeBySIMButton.TabIndex = 27;
            this.SummarizeBySIMButton.Text = "Summarize by SIM";
            this.SummarizeBySIMButton.UseVisualStyleBackColor = true;
            this.SummarizeBySIMButton.Click += new System.EventHandler(this.SummarizeBySIMButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 164);
            this.Controls.Add(this.lblPercentage);
            this.Controls.Add(this.InputPanel);
            this.Controls.Add(this.overAllProgressBar);
            this.Controls.Add(this.StepLabel);
            this.Name = "MainForm";
            this.Text = "CDR Processor";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.InputPanel.ResumeLayout(false);
            this.InputPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel InputPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnBrowseOutputFolder;
        private System.Windows.Forms.Button btnBrowseInput;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.TextBox txtInputFolder;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        public System.ComponentModel.BackgroundWorker progressUpdateBGWorker;
        private System.Windows.Forms.Label lblPercentage;
        private System.Windows.Forms.ProgressBar overAllProgressBar;
        private System.Windows.Forms.Label StepLabel;
        private System.Windows.Forms.Button btnExtractor;
        private System.Windows.Forms.TextBox txtExtractor;
        private System.Windows.Forms.Button SummarizeBySIMButton;
        
    }
}

