//--------------------------------------------------------------------------------------
// <copyright file="Logging.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Utility
{
    using System;
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.Net.Mail;
    using System.Web;

    using Trooper.ActiveDirectory;

    /// <summary>
    /// Application logging helper methods.
    /// </summary>
    [ContractVerification(true)]
    public static class Logging
    {
        /// <summary>
        /// Report application exceptions. Use this from the Application_Error method
        /// on the global AXA (or anywhere) to report the exception to the email
        /// address specified in the web or app config file. The following app settings should be present:
        /// AppName: The application name
        /// AppMode: Development, Staging etc
        /// AppTechnician: The email address to send the error
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public static void ReportApplicationException(Exception exception)
        {
            if (exception is HttpUnhandledException && exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            if (exception.Message.StartsWith("This is an invalid webresource request")
                || exception.Message.StartsWith("ASP.NET session has expired or could not be found")
                || exception.Message.StartsWith("The state information is invalid for this page and might be corrupted"))
            {
                return;
            }

            var user = new ActiveDirectoryUser();
            var subject = string.Format("{0} [ Error ]", ConfigurationManager.AppSettings["AppName"]);
            var to = ConfigurationManager.AppSettings["AppTechnician"];
            var body = string.Format(
                "AppMode: {0}\nUser: {1} ({2})\nError follows:\n{3}",
                ConfigurationManager.AppSettings["AppMode"],
                user.UserName,
                user.FullName,
                exception);

            var mm = new MailMessage("trooper@yourapp.com.au", to, subject, body);
            new SmtpClient().Send(mm);
        }
    }
}