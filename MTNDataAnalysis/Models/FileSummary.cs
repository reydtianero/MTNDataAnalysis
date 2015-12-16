// -----------------------------------------------------------------------
// <copyright file="FileSummary.cs" company="Global Supply Chain Services (Ltd)">
//     Copyright (c) GlobalTrack. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MTNDataAnalysis.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Descriptor for each file processed
    /// </summary>
    public class FileSummary
    {
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the header count.
        /// </summary>
        /// <value>
        /// The header count.
        /// </value>
        public int HeaderCount { get; set; }

        /// <summary>
        /// Gets or sets the records processed.
        /// </summary>
        /// <value>
        /// The records processed.
        /// </value>
        public int RecordsProcessed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FileSummary" /> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        /// <value>
        /// The remarks.
        /// </value>
        public string Remarks { get; set; }
    }
}
