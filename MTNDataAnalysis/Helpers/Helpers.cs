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
        /// Directories the has permission.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="accessRight">The access right.</param>
        /// <returns>True if user has the Access Right to the Directory Path</returns>
        public static bool DirectoryHasPermission(string directoryPath, FileSystemRights accessRight)
        {
            var result = false; 
            if (string.IsNullOrEmpty(directoryPath)) 
            {
                result = false;
            }

            AuthorizationRuleCollection rules = Directory.GetAccessControl(directoryPath).GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            foreach (FileSystemAccessRule rule in rules)
            {
                if (identity.Groups.Contains(rule.IdentityReference))
                {
                    if ((accessRight & rule.FileSystemRights) == accessRight)
                    {
                        if (rule.AccessControlType == AccessControlType.Allow)
                        { 
                            result = true; 
                        }
                    }
                }
            }

            return result;
        }

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
