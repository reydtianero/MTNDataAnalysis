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
    public class SummarizeStep : BaseHandler<CallDataRecordContext>
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

            context.EndTime = DateTime.Now;
            context.CallDataSummary = callDataSummary;
            
            if (base.Successor != null)
            {
                Successor.Process(context);
            }
        }

       

        /// <summary>
        /// Gets the summary by phone number.
        /// </summary>
        /// <param name="callDataRecords">The call data records.</param>
        /// <returns>Summarized Call Data by PhoneNumber</returns>
        private IEnumerable<CallDataSummary> GetSummaryByPhoneNumber(List<CallData> callDataRecords)
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

        /// <summary>
        /// Gets the summary by call imei.
        /// </summary>
        /// <param name="callDataRecords">The call data records.</param>
        /// <returns>Summarized Call Data by CallIMEI</returns>
        private IEnumerable<CallDataSummary> GetSummaryByCallIMEI(List<CallData> callDataRecords)
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
