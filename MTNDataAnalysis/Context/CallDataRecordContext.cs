using MTNDataAnalysis.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTNDataAnalysis.Context
{
    public class CallDataRecordContext : BaseContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallDataRecordContext"/> class.
        /// </summary>
        /// <param name="inputPath">The input path.</param>
        /// <param name="extractorFilePath">The extractor file path.</param>
        public CallDataRecordContext(string inputPath, string extractorFilePath)
        {
            this.InputPath = inputPath;
            this.ExtractorFilePath = extractorFilePath;
        }

        /// <summary>
        /// Occurs when Process Progress changed.
        /// </summary>
        public event Action<int> ProcessProgressChanged;

        /// <summary>
        /// Occurs when Process Progress changed.
        /// </summary>
        public event Action<string, bool> ProcessStepChanged;

        /// <summary>
        /// Gets or sets the location of the files to be procesed
        /// </summary>
        /// <value>
        /// The input path.
        /// </value>
        public string InputPath { get; set; }

        /// <summary>
        /// Gets or sets the complete path of the third party decompressor
        /// </summary>
        /// <value>
        /// The extractor file path.
        /// </value>
        public string ExtractorFilePath { get; set; }

        /// <summary>
        /// Gets or sets the call data records.
        /// </summary>
        /// <value>
        /// The call data records.
        /// </value>
        public IList<CallData> CallDataRecords { get; set; }

        /// <summary>
        /// Gets or sets the call data summary.
        /// </summary>
        /// <value>
        /// The call data summary.
        /// </value>
        public IEnumerable<CallDataSummary> CallDataSummary { get; set; }

        /// <summary>
        /// Gets or sets the processing result of each file
        /// </summary>
        /// <value>
        /// The files summary.
        /// </value>
        public IList<FileSummary> FileSummary { get; set; }

        /// <summary>
        /// Gets the staging path.
        /// </summary>
        /// <value>
        /// The staging path.
        /// </value>
        public string StagingPath
        {
            get
            {
                return this.OutputPath + "\\Staging";
            }
        }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        /// <value>
        /// The output path.
        /// </value>
        public string OutputPath { get; set; }

        /// <summary>
        /// Called when [process step changed].
        /// </summary>
        /// <param name="step">The step.</param>
        public void OnProcessStepChanged(string step, bool abort)
        {
            var evt = this.ProcessStepChanged;
            if (evt != null)
            {
                evt(step, abort);
            }
        }

        /// <summary>
        /// Called when [process progress changed].
        /// </summary>
        /// <param name="progress">The progress.</param>
        public void OnProcessProgressChanged(int progress)
        {
            var evt = this.ProcessProgressChanged;
            if (evt != null)
            {
                evt(progress);
            }
        }

        /// <summary>
        /// Gets or sets the [group by] field for the summary.
        /// </summary>
        /// <value>
        /// The group by field.
        /// </value>
        public string GroupByField { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime EndTime { get; set; }
    }
}
