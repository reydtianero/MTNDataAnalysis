// -----------------------------------------------------------------------
// <copyright file="Initializer.cs" company="Global Supply Chain Services (Ltd)">
//     Copyright (c) GlobalTrack. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MTNDataAnalysis.Chain
{
    using System.IO;
    using System.Security.AccessControl;
    using MTNDataAnalysis;
    using MTNDataAnalysis.Context;
    using MTNDataAnalysis.Helpers;

    /// <summary>
    /// Check for folder access and create staging directory
    /// </summary>
    public class InitializeStep : BaseHandler<CallDataRecordContext>
    {
        /// <summary>
        /// Processes this instance.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Process(CallDataRecordContext context)
        {
            var isReady = false;
            context.OnProcessStepChanged("Initializing...", false);
            try
            {

                var thisDirectory = new DirectoryInfo(context.InputPath);

                if (context.OutputPath == string.Empty)
                {
                    context.OutputPath = thisDirectory.CreateSubdirectory("Output").FullName;

                }
                else
                {
                    Directory.CreateDirectory(context.OutputPath);
                }

                var outputDiretory = new DirectoryInfo(context.OutputPath);
                outputDiretory.CreateSubdirectory("Staging");

                isReady = true;
            }
            catch
            {
                isReady = false;
            }

            if (this.Successor != null && isReady)
            {
                this.Successor.Process(context);
            }
            else
            {
                context.OnProcessStepChanged("There was a problem preparing the folders, please check permissions.", true);
            }
        }
    }
}
