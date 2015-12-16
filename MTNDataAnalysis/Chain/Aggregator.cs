// -----------------------------------------------------------------------
// <copyright file="Aggregator.cs" company="Global Supply Chain Services (Ltd)">
//     Copyright (c) GlobalTrack. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MTNDataAnalysis.Chain
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using MTNDataAnalysis.Context;
    using MTNDataAnalysis.Models;
    using MTNDataAnalysis.Helpers;

    /// <summary>
    /// Aggregate the data
    /// </summary>
    public class Aggregator : BaseHandler<CallDataRecordContext>
    {
        /// <summary>
        /// The context of the chain
        /// </summary>
        private CallDataRecordContext context;

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Process(CallDataRecordContext context)
        {
            this.context = context;
            List<CallData> callDataRecords = this.context.CallDataRecords.ToList();
            IEnumerable<CallDataSummary> callDataSummary = new List<CallDataSummary>();

            context.OnProcessStepChanged("Creating Report...", false);

            if (context.GroupByField == "PhoneNumber")
            {
                callDataSummary = GetSummaryByPhoneNumber(callDataRecords);
            }
            else if (context.GroupByField == "IMEI")
            {
                callDataSummary = GetSummaryByCallIMEI(callDataRecords);
            }

            WriteToFile(callDataSummary);

            if (base.Successor != null)
            {
                Successor.Process(context);
            }
        }

        private void WriteToFile(IEnumerable<CallDataSummary> callDataSummary)
        {
            string filename = this.context.OutputPath + "\\CDR_Summary_By_" + context.GroupByField + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
            StreamWriter fileWriter = File.AppendText(filename);
            fileWriter.WriteLine("Billing Period," + context.GroupByField + ", Data Volume, In Bytes");

            foreach (var c in callDataSummary)
            {
                fileWriter.WriteLine(c.BillingPeriod + ", " + c.GroupingField.ToString() + "," + c.HumanReadableSum.ToString() + "," + c.SumInBytes.ToString());
            }

            fileWriter.Close();

            System.Diagnostics.Process.Start(this.context.OutputPath);
        }


        public static IEnumerable<CallDataSummary> GetSummaryByPhoneNumber(List<CallData> callDataRecords)
        {
            var result = from r in callDataRecords
                         group r by new
                         {
                             r.BillingPeriod,
                             r.PhoneNumber
                         } into gcdr
                         select new CallDataSummary()
                       {
                           BillingPeriod = gcdr.Key.BillingPeriod,
                           GroupingField = gcdr.Key.PhoneNumber,
                           HumanReadableSum = Helpers.BytesToString((long)gcdr.Sum(r => r.DataVolume)),
                           SumInBytes = (long)gcdr.Sum(r => r.DataVolume)
                       };
            return result;
        }


        public static IEnumerable<CallDataSummary> GetSummaryByCallIMEI(List<CallData> callDataRecords)
        {
            var result = (from r in callDataRecords
                          group r by new
                          {
                              r.BillingPeriod,
                              r.CallIMEI
                          } into gcdr

                          select new CallDataSummary()
                          {
                              BillingPeriod = gcdr.Key.BillingPeriod,
                              GroupingField = gcdr.Key.CallIMEI,
                              HumanReadableSum = Helpers.BytesToString((long)gcdr.Sum(r => r.DataVolume)),
                              SumInBytes = (long)gcdr.Sum(r => r.DataVolume)
                          }).OrderBy(r => r.GroupingField);
            return result;
        }
    }
}
