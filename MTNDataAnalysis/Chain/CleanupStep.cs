// -----------------------------------------------------------------------
// <copyright file="CleanupStep.cs" company="YouSource Inc.">
//     Copyright (c) YouSource Inc.. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MTNDataAnalysis.Chain
{
    using System.IO;
    using System.Security.AccessControl;
    using MTNDataAnalysis.Context;
    using MTNDataAnalysis.Helpers;

    /// <summary>
    /// Deletes staging data(i.e. decompressed files from archive)
    /// </summary>
    public class CleanupStep : BaseHandler<CallDataRecordContext>
    {
        /// <summary>
        /// Delete staging data
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Process(CallDataRecordContext context)
        {
            if (Directory.Exists(context.StagingPath))
            {
                context.OnProcessStepChanged("Cleaning Up...", false);
                var thisDirectory = new DirectoryInfo(context.StagingPath);
                thisDirectory.Delete(true);
                context.OnProcessStepChanged("Done!", true);
            }
        }
    }
}
