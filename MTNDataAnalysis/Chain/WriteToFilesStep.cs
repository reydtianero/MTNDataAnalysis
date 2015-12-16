// -----------------------------------------------------------------------
// <copyright file="WriteToFilesStep.cs" company="Global Supply Chain Services (Ltd)">
//     Copyright (c) GlobalTrack. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MTNDataAnalysis.Chain
{
    using MTNDataAnalysis.Context;
    using MTNDataAnalysis.Models;
    using OfficeOpenXml;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class WriteToFilesStep : BaseHandler<CallDataRecordContext>
    {
        private ExcelPackage package;
        private ExcelWorkbook workbook;
        
        private CallDataRecordContext context; 

        public override void Process(CallDataRecordContext context)
        {
            this.context = context;
            //WriteCallDataSummary();
            //WriteFileSummary();
            WriteToExcel();
            System.Diagnostics.Process.Start(this.context.OutputPath);
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

        private void WriteToExcel()
        {
            string filename = this.context.OutputPath + "\\Summary_By_" + this.context.GroupByField + "_" + this.context.StartTime.ToString("yyyyMMddhhmmss") + ".xlsx";
            
            this.package = new ExcelPackage();
            this.BuildCallSummarySheet();
            this.BuildFileSummarySheet();
            this.package.SaveAs(new FileInfo(filename));
        }

        private void BuildCallSummarySheet()
        {
            ExcelWorksheet worksheet = this.package.Workbook.Worksheets.Add("Call Data Summary");
            DataTable dataTable = new DataTable();

            worksheet.Cells[1, 1].Value = "Billing Period";
            worksheet.Cells[1, 2].Value = this.context.GroupByField;
            worksheet.Cells[1, 3].Value = "Data Volume";
            worksheet.Cells[1, 4].Value = "In Bytes";
            dataTable = ConvertToCallDataSummaryDataTable(this.context.CallDataSummary.ToList());

            worksheet.Cells["A2"].LoadFromDataTable(dataTable, false);
            worksheet.Column(1).AutoFit(0);
            worksheet.Column(2).AutoFit(0);
            worksheet.Column(3).AutoFit(0);
            worksheet.Column(4).AutoFit(0);
            //Create Header
        }

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

            dataTable = ConvertToFileSummaryDataTable(context.FileSummary);

            worksheet.Cells["A6"].LoadFromDataTable(dataTable, false);
            worksheet.Column(1).AutoFit(0);
            worksheet.Column(2).AutoFit(0);
            worksheet.Column(3).AutoFit(0);
            worksheet.Column(4).AutoFit(0);
            
        }

        private DataTable ConvertToFileSummaryDataTable(IList<FileSummary> fileSummary)
        {
            var result = new DataTable();

            result.Columns.Add("FileName", typeof(string));
            result.Columns.Add("RecordsFound", typeof(int));
            result.Columns.Add("RecordsProcessed", typeof(int));
            result.Columns.Add("Remarks", typeof(string));

            foreach (var f in fileSummary)
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

        private DataTable ConvertToCallDataSummaryDataTable(IList<CallDataSummary> callDataSummary)
        {
            var result = new DataTable();

            result.Columns.Add("BillingPeriod", typeof(string));
            result.Columns.Add( context.GroupByField, typeof(string));
            result.Columns.Add("DataVolume", typeof(string));
            result.Columns.Add("DataVolumeInBytes", typeof(long));

            foreach (var c in callDataSummary)
            {
                var row = result.NewRow();
                row["BillingPeriod"] = c.BillingPeriod;
                row[context.GroupByField] = c.GroupingField;
                row["DataVolume"] = c.HumanReadableSum;
                row["DataVolumeInBytes"] = c.SumInBytes;
                result.Rows.Add(row);
            }

            return result;
        }
    }

}
