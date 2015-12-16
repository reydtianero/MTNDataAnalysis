// -----------------------------------------------------------------------
// <copyright file="CallData.cs" company="YouSource Inc.">
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
    /// Data Model for line item in the call data record(CDR)
    /// </summary>
    public class CallData
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the call date.
        /// </summary>
        /// <value>
        /// The call date.
        /// </value>    
        public DateTime CallDateTime { get; set; }

        /// <summary>
        /// Gets or sets the billing period.
        /// </summary>
        /// <value>
        /// The billing period.
        /// </value>
        public string BillingPeriod { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the call IEMI.
        /// </summary>
        /// <value>
        /// The call IEMI/Identifier.
        /// </value>
        public string CallIMEI { get; set; }

        /// <summary>
        /// Gets or sets the data volume.
        /// </summary>
        /// <value>
        /// The data volume.
        /// </value>
        public decimal DataVolume { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }
    }
}
