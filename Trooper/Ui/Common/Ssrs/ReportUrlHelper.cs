//--------------------------------------------------------------------------------------
// <copyright file="ReportUrlHelper.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Common.Ssrs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;
    using System.Web;

    using HtmlAgilityPack;

    /// <summary>
    /// Provides tools for redirecting to reports. Depends on Settings being correct
    /// </summary>
    public class ReportUrlHelper
    {
        /// <summary>
        /// The report cache name.
        /// </summary>
        private const string ReportCacheName = "Trooper.Ui.Common.Ssrs.CachedReports";

        /// <summary>
        /// Provides a URL for looking at all the reports in the SSRS
        /// </summary>
        /// <returns>
        /// Returns the URL
        /// </returns>
        public static string GetAllReportsUrl()
        {
            var path = Settings.ReportPath ?? string.Empty;

            if (path.Last() == '/')
            {
                path = path.Substring(0, path.Length - 1);
            }

            path = path.Replace("/", "%2f");
            path = path.Replace(" ", "+");

            return string.Format(
                "http://{0}/Reports/Pages/Folder.aspx?ItemPath={1}&ViewMode=List", Settings.ReportServer, path);
        }

        /// <summary>
        /// Returns the result of <see cref="GetAllReportUrls()"/> from a cache which will
        /// reduce the number of calls to the SSRS server. Use this to reduce load. 
        /// To refresh you can set the refresh property to true or restart the application
        /// pool.
        /// </summary>
        /// <param name="refresh">
        /// Force a refresh of the reports list.
        /// </param>
        /// <returns>
        /// List of report URLs
        /// </returns>
        public static List<ReportItem> GetAllReportUrlsCached(bool refresh = false)
        {
            List<ReportItem> cache;

            if (!refresh)
            {
                cache = HttpRuntime.Cache[ReportCacheName] as List<ReportItem>;

                if (cache != null)
                {
                    return cache;
                }
            }

            cache = GetAllReportUrls();

            HttpRuntime.Cache[ReportCacheName] = cache;

            return cache;
        }

        /// <summary>
        /// Get all the reports from the reporting folder for your project.
        /// Do this once on page load or try and cache the results.
        /// </summary>
        /// <remarks> 
        /// This is a pretty nasty little script. There are better methods using the 
        /// SSRS services but ours don't work or need special permissions or something...
        /// Anyway - we should prepare for this breaking in future releases of SSRS.
        /// </remarks>
        /// <returns>
        /// A list of ReportItem
        /// </returns>
        public static List<ReportItem> GetAllReportUrls()
        {
            var path = Settings.ReportPath ?? string.Empty;
            var result = new List<ReportItem>();

            if (string.IsNullOrEmpty(Settings.ReportServer))
            {
                return result;
            }

            if (path.Last() == '/')
            {
                path = path.Substring(0, path.Length - 1);
            }

            path = path.Replace("/", "%2f");
            path = path.Replace(" ", "+");

            var listUrl = string.IsNullOrEmpty(path) 
                ? string.Format("http://{0}/ReportServer/lists.asmx", Settings.ReportServer)
                : string.Format("http://{0}/ReportServer/lists.asmx?{1}&rs:Command=ListChildren", Settings.ReportServer, path);

            var wc = new WebClient { UseDefaultCredentials = true };
            var page = wc.OpenRead(new Uri(listUrl));
            var doc = new HtmlDocument();
            doc.Load(page);

            foreach (var a in doc.DocumentNode.SelectNodes("//body//a"))
            {
                var url = a.Attributes["href"];
                var name = a.InnerText;

                if (name == "[To Parent Directory]")
                {
                    continue;
                }

                if (url != null && name != null)
                {
                    result.Add(
                        new ReportItem
                        {
                            Name = name,
                            Url =
                                string.Format(
                                    "http://{0}/Reports/Pages/Report.aspx?ItemPath={1}",
                                    Settings.ReportServer,
                                    url.Value.Replace("?", string.Empty))
                        });
                }
            }

            return result;
        }
        
        /// <summary>
        /// Gets the URL for a report
        /// </summary>
        /// <param name="reportName">
        /// The report name.
        /// </param>
        /// <param name="reportPath">
        /// The report path.
        /// </param>
        /// <param name="parameters">The parameters. </param>
        /// <returns>
        /// The URL of the report
        /// </returns>
        public static string GetReportUrl(string reportName, string reportPath = null, object parameters = null)
        {
            var parametersStr = string.Empty;

            var format = parameters == null
                             ? "http://{0}/Reports/Pages/Report.aspx?ItemPath={1}{2}"
                             : "http://{0}/ReportServer?{1}{2}{3}&rs%3AParameterLanguage=en-AU";

            var path = reportPath ?? Settings.ReportPath;

            if (path.Last() != '/')
            {
                path += '/';
            }

            path = path.Replace("/", "%2F");
            path = path.Replace(" ", "+");
            reportName = reportName.Replace(" ", "+");

            if (parameters != null)
            {
                var properties = TypeDescriptor.GetProperties(parameters);
                parametersStr = properties.Cast<PropertyDescriptor>().Aggregate(
                    parametersStr,
                    (current, property) =>
                    current + string.Format("&{0}={1}", property.Name, property.GetValue(parameters)));
            }

            var url = string.Format(format, Settings.ReportServer, path, reportName, parametersStr);

            return url;
        }

        /// <summary>
        /// Redirects to the Report
        /// </summary>
        /// <param name="reportName">
        /// The report name.
        /// </param>
        /// <param name="reportPath">
        /// The report path.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public static void RedirectToReport(string reportName, string reportPath = null, object parameters = null)
        {
            HttpContext.Current.Response.Redirect(GetReportUrl(reportName, reportPath, parameters));
        }
    }
}
