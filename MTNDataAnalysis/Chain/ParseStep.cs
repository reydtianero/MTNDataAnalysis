// -----------------------------------------------------------------------
// <copyright file="ParseStep.cs" company="YouSource Inc.">
//     Copyright (c) YouSource Inc.. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MTNDataAnalysis.Chain
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using MTNDataAnalysis.Context;
    using MTNDataAnalysis.Models;
    
    /// <summary>
    /// Parses the files into the Enumerable in context
    /// </summary>
    public class ParseStep : BaseHandler<CallDataRecordContext>
    {
        /// <summary>
        /// The context
        /// </summary>
        private CallDataRecordContext context;

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Process(CallDataRecordContext context)
        {
            int fileCounter = 0;
            this.context = context;
            this.context.CallDataRecords = new List<CallData>();
            this.context.FileSummary = new List<FileSummary>();

            var files = Directory.GetFiles(this.context.StagingPath);
            context.OnProcessStepChanged("Parsing Files...", false);
            
            foreach (string f in files)
            {
                string fileRemarks = string.Empty;
                FileInfo fileInProcess = new FileInfo(f);
                this.context.OnProcessProgressChanged(Convert.ToInt32(Math.Round((double)++fileCounter / files.Length * 100)));
                if (this.ValidateFile(fileInProcess, out fileRemarks))
                {
                    this.ReadFileToModel(fileInProcess);
                }
                else
                {
                    FileSummary dataFileSummary = new FileSummary();
                    dataFileSummary.FileName = fileInProcess.Name;
                    dataFileSummary.Remarks = fileRemarks;
                    this.context.FileSummary.Add(dataFileSummary);
                }
            }

            if (this.Successor != null)
            {
                this.Successor.Process(this.context);
            }
        }

        /// <summary>
        /// Reads the file to model.
        /// </summary>
        /// <param name="file">The file.</param>
        private void ReadFileToModel(FileInfo file)
        {
            FileSummary dataFileSummary = new FileSummary();
            StreamReader fileReader = file.OpenText();
            CallData callData = default(CallData);
            
            var line = fileReader.ReadLine();
            int headerCount;
            int year;
            int month;
            int day;
            int rowCount = 0;

            int.TryParse(line.Substring(21, 10), out headerCount);
            dataFileSummary.FileName = file.Name;
            dataFileSummary.HeaderCount = headerCount;
            
            while ((line = fileReader.ReadLine())[0] != 'T')
            {
                callData = new CallData();
                callData.FileName = file.Name;
                callData.PhoneNumber = line.Substring(0, 11);
                callData.BillingPeriod = line.Substring(60, 5); 
                callData.CallIMEI = line.Substring(81, 15);
                
                int.TryParse(line.Substring(11, 4), out year);
                int.TryParse(line.Substring(15, 2), out month);
                int.TryParse(line.Substring(17, 2), out day);

                callData.CallDateTime = new DateTime(year, month, day).Add(TimeSpan.Parse(line.Substring(19, 8)));
                callData.DataVolume = Convert.ToDecimal(line.Substring(103, 14));

                this.context.CallDataRecords.Add(callData);
                rowCount++;
            }

            dataFileSummary.RecordsProcessed = rowCount;
            dataFileSummary.Success = true;
            
            this.context.FileSummary.Add(dataFileSummary);
            fileReader.Close();
        }

        /// <summary>
        /// Validates the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="fileErrors">The file errors.</param>
        /// <returns> true / false </returns>
        private bool ValidateFile(FileInfo file, out string fileErrors)
        {
            StreamReader fileReader = file.OpenText();
            fileErrors = string.Empty;
            var isValid = true;

            try
            {
                if (!this.CheckHeader(fileReader))
                {
                    isValid = false;
                    fileErrors = "Invalid File Header";
                }

                if (!this.CheckTrailer(fileReader))
                {
                    isValid = false;
                    fileErrors = fileErrors + ";" + "Invalid File Trailer";
                }
            }
            catch
            {
                isValid = false;
                fileErrors = fileErrors + ";" + "Invalid File Format";
            }

            return isValid;
        }

        /// <summary>
        /// Checks the trailer.
        /// </summary>
        /// <param name="fileReader">The file reader.</param>
        /// <returns>true / false</returns>
        private bool CheckTrailer(StreamReader fileReader)
        {
            var isValid = true;

            fileReader.DiscardBufferedData();
            fileReader.BaseStream.Seek(-2, SeekOrigin.End);

            string s = fileReader.ReadToEnd();
            isValid = s[0] == 'T' ? true : false;

            return isValid;
        }

        /// <summary>
        /// Checks the header.
        /// </summary>
        /// <param name="fileReader">The file reader.</param>
        /// <returns>true when header is valid</returns>
        private bool CheckHeader(StreamReader fileReader)
        {
            var isValid = true;
            var header = fileReader.Peek() != -1 ? (char)fileReader.Peek() : default(char);
            isValid = header == 'H' ? true : false;
            return isValid;
        }
    }
}
