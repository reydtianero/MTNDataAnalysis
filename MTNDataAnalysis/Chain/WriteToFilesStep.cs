// -----------------------------------------------------------------------
// <copyright file="WriteToFilesStep.cs" company="YouSource Inc.">
//     Copyright (c) YouSource Inc.. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MTNDataAnalysis.Chain
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using MTNDataAnalysis.Context;
    using MTNDataAnalysis.Models;
    using OfficeOpenXml;

    /// <summary>
    /// Outputs the result of the chain to file
    /// </summary>
    public class WriteToFilesStep : BaseHandler<CallDataRecordContext>
    {
        /// <summary>
        /// The Excel package
        /// </summary>
        private ExcelPackage package;

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
            this.context = context;
            ////WriteCallDataSummary();
            ////WriteFileSummary();
            this.WriteToExcel();
            System.Diagnostics.Process.Start(this.context.OutputPath);

            if (this.Successor != null)
            {
                this.Successor.Process(this.context);
            }
        }

        /// <summary>
        /// Writes the summary to file.
        /// </summary>
        private void WriteCallDataSummary()
        {
            string filename = this.context.OutputPath + "\\CDR_Summary_By_" + this.context.GroupByField + "_" + this.context.StartTime.ToString("yyyyMMddhhmmss") + ".csv";
            StreamWriter fileWriter = File.AppendText(filename);
            fileWriter.WriteLine("Billing Period," + this.context.GroupByField + ", Data Volume, In Bytes");

            foreach (var c in this.context.CallDataSummary)
            {
                fileWriter.WriteLine(c.BillingPeriod + ", " + c.GroupingField.ToString() + "," + c.HumanReadableSum.ToString() + "," + c.SumInBytes.ToString());
            }

            fileWriter.Close();
        }

        /// <summary>
        /// Writes the file summary.
        /// </summary>
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
                fileWriter.WriteLine(f.FileName + ", " + f.HeaderCount.ToString() + "," + f.RecordsProcessed.ToString() + "," + (f.Success ? "Success" : f.Remarks));
            }

            fileWriter.Close();

            System.Diagnostics.Process.Start(this.context.OutputPath);
        }

        /// <summary>
        /// Writes to excel.
        /// </summary>
        private void WriteToExcel()
        {
            string filename = this.context.OutputPath + "\\Summary_By_" + this.context.GroupByField + "_" + this.context.StartTime.ToString("yyyyMMddhhmmss") + ".xlsx";
            
            this.package = new ExcelPackage();
            this.BuildCallSummarySheet();
            this.BuildFileSummarySheet();
            this.package.SaveAs(new FileInfo(filename));
        }

        /// <summary>
        /// Builds the call summary sheet.
        /// </summary>
        private void BuildCallSummarySheet()
        {
            ExcelWorksheet worksheet = this.package.Workbook.Worksheets.Add("Call Data Summary");
            DataTable dataTable = new DataTable();

            worksheet.Cells[1, 1].Value = "Billing Period";
            worksheet.Cells[1, 2].Value = this.context.GroupByField;
            worksheet.Cells[1, 3].Value = "Data Volume";
            worksheet.Cells[1, 4].Value = "In Bytes";
            dataTable = this.ConvertToCallDataSummaryDataTable();

            worksheet.Cells["A2"].LoadFromDataTable(dataTable, false);
            worksheet.Column(1).AutoFit(0);
            worksheet.Column(2).AutoFit(0);
            worksheet.Column(3).AutoFit(0);
            worksheet.Column(4).AutoFit(0);
        }

        /// <summary>
        /// Builds the file summary sheet.
        /// </summary>
        private void BuildFileSummarySheet()
        {
            ExcelWorksheet worksheet = this.package.Workbook.Worksheets.Add("File Summary");
            DataTable dataTable = new DataTable();

            worksheet.Cells[1, 1].Value = "Process Started: ";
            worksheet.Cells[1, 2].Value = this.context.StartTime.ToString("yyyy-MM-dd hh:mm:ss");

            worksheet.Cells[2, 1].Value = "Process Ended: ";
            worksheet.Cells[2, 2].Value = this.context.EndTime.ToString("yyyy-MM-dd hh:mm:ss");

            worksheet.Cells[3, 1].Value = "Total Time: ";
            worksheet.Cells[3, 2].Value = this.context.EndTime.Subtract(this.context.StartTime).TotalSeconds.ToString("#,#.00#") + "Second(s)";
            
            worksheet.Cells[5, 1].Value = "Filename";
            worksheet.Cells[5, 2].Value = "Records Found";
            worksheet.Cells[5, 3].Value = "Records Processed";
            worksheet.Cells[5, 4].Value = "Remarks";

            dataTable = this.ConvertToFileSummaryDataTable();

            worksheet.Cells["A6"].LoadFromDataTable(dataTable, false);
            worksheet.Column(1).AutoFit(0);
            worksheet.Column(2).AutoFit(0);
            worksheet.Column(3).AutoFit(0);
            worksheet.Column(4).AutoFit(0);
        }

        /// <summary>
        /// Converts to file summary data table.
        /// </summary>
        /// <returns> file summary in data table</returns>
        private DataTable ConvertToFileSummaryDataTable()
        {
            var result = new DataTable();

            result.Columns.Add("FileName", typeof(string));
            result.Columns.Add("RecordsFound", typeof(int));
            result.Columns.Add("RecordsProcessed", typeof(int));
            result.Columns.Add("Remarks", typeof(string));

            foreach (var f in this.context.FileSummary)
            {
                var row = result.NewRow();
                row["FileName"] = f.FileName;
                row["RecordsFound"] = f.HeaderCount;
                row["RecordsProcessed"] = f.RecordsProcessed;
                row["Remarks"] = f.Success ? "Success" : f.Remarks;
                result.Rows.Add(row);
            }

            return result;
        }

        /// <summary>
        /// Converts to call data summary data table.
        /// </summary>
        /// <returns>Call data summary in data table</returns>
        private DataTable ConvertToCallDataSummaryDataTable()
        {
            var result = new DataTable();

            result.Columns.Add("BillingPeriod", typeof(string));
            result.Columns.Add(this.context.GroupByField, typeof(string));
            result.Columns.Add("DataVolume", typeof(string));
            result.Columns.Add("DataVolumeInBytes", typeof(long));

            foreach (var c in this.context.CallDataSummary.ToList())
            {
                var row = result.NewRow();
                row["BillingPeriod"] = c.BillingPeriod;
                row[this.context.GroupByField] = c.GroupingField;
                row["DataVolume"] = c.HumanReadableSum;
                row["DataVolumeInBytes"] = c.SumInBytes;
                result.Rows.Add(row);
            }

            return result;
        }
    }
}
