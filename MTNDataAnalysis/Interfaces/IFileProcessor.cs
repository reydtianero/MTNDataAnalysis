// -----------------------------------------------------------------------
// <copyright file="IFileProcessor.cs" company="Global Supply Chain Services (Ltd)">
//     Copyright (c) GlobalTrack. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MTNDataAnalysis.Interfaces
{
    using System;
    using MTNDataAnalysis.Context;

    /// <summary>
    /// Interface for File Processing steps
    /// </summary>
    /// <typeparam name="T">Context Type</typeparam>
    public interface IFileProcessor<T> where T : BaseContext
    {
        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
         void Process(T context);

         /// <summary>
         /// Sets the successor.
         /// </summary>
         /// <param name="successor">The successor.</param>
         void SetSuccessor(IFileProcessor<T> successor);
    }
}
