// -----------------------------------------------------------------------
// <copyright file="BaseHandler.cs" company="YouSource Inc.">
//     Copyright (c) YouSource Inc.. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MTNDataAnalysis.Chain
{
    using MTNDataAnalysis.Context;
    using MTNDataAnalysis.Interfaces;

    /// <summary>
    /// Concrete class for Handlers
    /// </summary>
    /// <typeparam name="T">Context Type</typeparam>
    public abstract class BaseHandler<T> : IFileProcessor<T> where T : BaseContext
    {
        /// <summary>
        /// Gets the successor.
        /// </summary>
        /// <value>
        /// The successor.
        /// </value>
        protected internal IFileProcessor<T> Successor { get; private set; }

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public abstract void Process(T context);

        /// <summary>
        /// Sets the successor.
        /// </summary>
        /// <param name="successor">The successor.</param>
        public void SetSuccessor(IFileProcessor<T> successor)
        {
           if (this.Successor == null)
            {
                this.Successor = successor;
            }
            else
            {
                this.Successor.SetSuccessor(successor);
            }
        }
    }
}
