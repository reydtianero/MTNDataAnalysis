// -----------------------------------------------------------------------
// <copyright file="CallDataSummary.cs" company="YouSource Inc.">
//     Copyright (c) YouSource Inc.. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MTNDataAnalysis.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Model for the summarized data row
    /// </summary>
    public class CallDataSummary
    {
        /// <summary>
        /// Gets or sets the billing period.
        /// </summary>
        /// <value>
        /// The billing period.
        /// </value>
        public string BillingPeriod { get; set; }

        /// <summary>
        /// Gets or sets the grouping field.
        /// </summary>
        /// <value>
        /// The grouping field.
        /// </value>
        public string GroupingField { get; set; }

        /// <summary>
        /// Gets or sets the human readable sum.
        /// </summary>
        /// <value>
        /// The human readable sum.
        /// </value>
        public string HumanReadableSum { get; set; }

        /// <summary>
        /// Gets or sets the sum in bytes.
        /// </summary>
        /// <value>
        /// The sum in bytes.
        /// </value>
        public long SumInBytes { get; set; }
    }
}
