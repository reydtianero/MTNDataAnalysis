// -----------------------------------------------------------------------
// <copyright file="Cleaner.cs" company="Global Supply Chain Services (Ltd)">
//     Copyright (c) GlobalTrack. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace MTNDataAnalysis.Chain
{
    using System.IO;
    using System.Security.AccessControl;
    using MTNDataAnalysis.Context;
    using MTNDataAnalysis.Helpers;

    public class Cleaner : BaseHandler<CallDataRecordContext>
    {
        public override void Process(CallDataRecordContext context)
        {

            if (Directory.Exists(context.StagingPath)) //&& IOHelpers.DirectoryHasPermission(context.StagingPath, FileSystemRights.DeleteSubdirectoriesAndFiles))
            {
                context.OnProcessStepChanged("Cleaning Up...", false);
                var thisDirectory = new DirectoryInfo(context.StagingPath);
               // thisDirectory.Delete(true);
                context.OnProcessStepChanged("Done!", true);
            }
        }

    }
}
