// -----------------------------------------------------------------------
// <copyright file="ExtractArchivesStep.cs" company="YouSource Inc.">
//     Copyright (c) YouSource Inc.. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MTNDataAnalysis.Chain
{
    using System;
    using System.IO;
    using Ebixio.LZW;
    using MTNDataAnalysis.Context;
    
    /// <summary>
    /// Extracts the files
    /// </summary>
    public class ExtractArchivesStep : BaseHandler<CallDataRecordContext>
    {
        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Process(CallDataRecordContext context)
        {
            var fileCounter = 0;

            DirectoryInfo directorySelected = new DirectoryInfo(context.InputPath);
            context.OnProcessStepChanged("Extracting Files...", false);

            var files = directorySelected.GetFiles("*.Z");
            if (files.Length > 0)
            {
                foreach (FileInfo fileToDecompress in files)
                {
                    try
                    {
                        this.Decompress(fileToDecompress, context.StagingPath);
                    }
                    catch 
                    { 
                        ////Skip File
                    }

                    context.OnProcessStepChanged("Extracting from archive:" + fileToDecompress.Name, false);
                    context.OnProcessProgressChanged(Convert.ToInt32(Math.Round((double)++fileCounter / files.Length * 100)));
                }

                if (this.Successor != null && fileCounter > 0)
                {
                    this.Successor.Process(context);
                }
            }
            else
            {
                context.OnProcessStepChanged("No source files were found", true);
            }
        }

        /// <summary>
        /// Decompresses the specified file to decompress.
        /// </summary>
        /// <param name="extractorFilePath">The extractor file path.</param>
        /// <param name="fileToDecompress">The file to decompress.</param>
        /// <param name="outputFolder">The output folder.</param>
        private void Decompress(string extractorFilePath, FileInfo fileToDecompress, string outputFolder)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = extractorFilePath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-y x " + fileToDecompress.FullName.ToString() + " -o" + outputFolder;
            p.Start();
            p.WaitForExit();
        }

        /// <summary>
        /// Decompresses the specified file to decompress using LZW Decompression .
        /// </summary>
        /// <param name="fileToDecompress">The file to decompress.</param>
        /// <param name="outputFolder">The output folder.</param>
        private void Decompress(FileInfo fileToDecompress, string outputFolder)
        {
            byte[] buffer = new byte[4096];
            string outFile = outputFolder + "\\" + Path.GetFileNameWithoutExtension(fileToDecompress.Name);

            using (Stream inStream = new LzwInputStream(File.OpenRead(fileToDecompress.FullName)))
            {
                using (FileStream outStream = File.Create(outFile))
                {
                    int read;
                    while ((read = inStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outStream.Write(buffer, 0, read);
                    }
                }
            }
        }
    }
}
