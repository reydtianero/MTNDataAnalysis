// -----------------------------------------------------------------------
// <copyright file="IOHelpers.cs" company="Global Supply Chain Services (Ltd)">
//     Copyright (c) GlobalTrack. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace MTNDataAnalysis.Helpers
{
    using System;
    using System.IO;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using System.Windows.Forms;

    /// <summary>
    /// Input/Output File System Helpers
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Sets the property in GUI thread.
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="control">The control.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentException">
        /// The 'property' expression must specify a property on the control.
        /// or
        /// The 'property' expression must specify a property on the control.
        /// </exception>
        public static void SetPropertyInGuiThread<C, V>(this C control, Expression<Func<C, V>> property, V value) where C : Control
        {
            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("The 'property' expression must specify a property on the control.");

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException("The 'property' expression must specify a property on the control.");

            if (control.InvokeRequired)
                control.Invoke(
                    (Action<C, Expression<Func<C, V>>, V>)SetPropertyInGuiThread,
                    new object[] { control, property, value }
                );
            else
                propertyInfo.SetValue(control, value, null);
        }

        /// <summary>
        /// Byteses to string.
        /// </summary>
        /// <param name="byteCount">The byte count.</param>
        /// <returns>Returns a human readable file size</returns>
        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long Kbytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(Kbytes, 1024)));
            double num = Math.Round(Kbytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

    }
}
