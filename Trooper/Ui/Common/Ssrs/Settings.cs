//--------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Common.Ssrs
{
    using System.Configuration;

    /// <summary>
    /// This project uses these APP Settings values
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Gets Reporting Server.
        /// </summary>
        public static string ReportServer
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportServer"];
            }
        }

        /// <summary>
        /// Gets Report Path.
        /// </summary>
        public static string ReportPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportPath"];
            }
        }
    }
}
