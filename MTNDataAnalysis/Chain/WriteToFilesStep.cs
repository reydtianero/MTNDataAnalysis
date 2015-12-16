// -----------------------------------------------------------------------
// <copyright file="WriteToFilesStep.cs" company="Global Supply Chain Services (Ltd)">
//     Copyright (c) GlobalTrack. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MTNDataAnalysis.Chain
{
    using MTNDataAnalysis.Context;
    using MTNDataAnalysis.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class WriteToFilesStep : BaseHandler<CallDataRecordContext>
    {
        private CallDataRecordContext context; 
        public override void Process(CallDataRecordContext context)
        {
            this.context = context;
            WriteCallDataSummary();
            WriteFileSummary();
            if (Successor != null)
            {
                Successor.Process(context);
            }
        }

        /// <summary>
        /// Writes the summary to file.
        /// </summary>
        /// <param name="callDataSummary">The call data summary.</param>
        private void WriteCallDataSummary()
        {
            string filename = this.context.OutputPath + "\\CDR_Summary_By_" + this.context.GroupByField + "_" + this.context.StartTime.ToString("yyyyMMddhhmmss") + ".csv";
            StreamWriter fileWriter = File.AppendText(filename);
            fileWriter.WriteLine("Billing Period," + context.GroupByField + ", Data Volume, In Bytes");

            foreach (var c in this.context.CallDataSummary)
            {
                fileWriter.WriteLine(c.BillingPeriod + ", " + c.GroupingField.ToString() + "," + c.HumanReadableSum.ToString() + "," + c.SumInBytes.ToString());
            }

            fileWriter.Close();

            System.Diagnostics.Process.Start(this.context.OutputPath);
        }

        private void WriteFileSummary()
        {
            string filename = this.context.OutputPath + "\\Files_Summary_By_" + this.context.GroupByField + "_" + this.context.StartTime.ToString("yyyyMMddhhmmss") + ".csv";
            StreamWriter fileWriter = File.AppendText(filename);
            fileWriter.WriteLine("Process Started: {0}", this.context.StartTime.ToString("yyyy-MM-dd hh:mm:ss"));
            fileWriter.WriteLine("Process Ended: {0}", this.context.EndTime.ToString("yyyy-MM-dd hh:mm:ss"));
            fileWriter.WriteLine("Total Time: {0} Second(s)", this.context.EndTime.Subtract(this.context.StartTime).TotalSeconds.ToString("#,#.00#"));
            fileWriter.WriteLine("Filename, Records Found, Records Processed, Remarks");

            foreach (var f in this.context.FileSummary)
            {
                fileWriter.WriteLine(f.FileName+ ", " + f.HeaderCount.ToString() + "," + f.RecordsProcessed.ToString() + "," + (f.Success ? "Success" : f.Remarks));
            }

            fileWriter.Close();

            System.Diagnostics.Process.Start(this.context.OutputPath);
        }
    }

}
