//--------------------------------------------------------------------------------------
// <copyright file="CruncherController.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Cruncher
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    using Trooper.Ui.Mvc.Utility;
    using Trooper.Utility;

    using dotless.Core;

    using Microsoft.Ajax.Utilities;
    using Trooper.Ui.Interface.Mvc.Cruncher;

    /// <summary>
    /// The controller class that handles requests for the JavaScript and Less/CSS files that are collected
    /// by the Cruncher class.
    /// </summary>
    public class CruncherController : Controller, ICruncherController
    {      

        /// <summary>
        /// Gets the source that should be returned based on the area for the store with the given ID
        /// </summary>
        /// <param name="area">
        /// The area.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The JavaScript of Less source
        /// </returns>
        [Compress]
        [HttpGet]
        public ActionResult GetSources(MimeTypes mimeType, Guid id)
        {
            var cruncherStore = Cruncher.GetStore(id);

            if (cruncherStore == null)
            {
                return this.Content(string.Format("/* No store found for store {0} */", id), "text/javascript");
            }

            switch (mimeType)
            {
                case MimeTypes.Js:
                    return this.GetContent(cruncherStore.Js(), MimeTypes.Js);

                case MimeTypes.Css:
                    return this.GetContent(cruncherStore.Css(), MimeTypes.Css);
            }

            return this.Content(string.Empty);
        }

        /// <summary>
        /// Creates an alert in the JavaScript stream. Will alert the developer to a missing source.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string JavascriptAlert(string message)
        {
            if (message == null)
            {
                return string.Empty;
            }

            message = message.Replace(@"\", @"\\");
            message = message.Replace("'", "\'");

            return string.Format("alert('Cruncher Error:\\n----------------\\n{0}');", message);
        }

        /// <summary>
        /// Takes matches from GetFileContent and GetInlineContent where URL references have been discovered
        /// and corrects those URLs so they will correctly from the supplied basePath
        /// </summary>
        /// <param name="m">
        /// The m.
        /// </param>
        /// <param name="basePath">
        /// The base path.
        /// </param>
        /// <returns>
        /// Corrected match content
        /// </returns>
        private string Matcher(Match m, string basePath)
        {
            var match = Regex.Match(m.Value, @"(?<=\().*(?=\))");

            if (!match.Success)
            {
                return m.Value;
            }

            var url = match.ToString().Replace(@"'", string.Empty).Replace("\"", string.Empty);

            if (url.StartsWith("../"))
            {
                var relPath = basePath;

                while (url.StartsWith("../"))
                {
                    relPath = relPath.Substring(0, relPath.LastIndexOf('/'));
                    url = url.Replace("../", string.Empty);
                    url = string.Format("{0}/{1}", relPath, url);
                }
            }
            else
            {
                url = string.Format("{0}/{1}", basePath, url);
            }

            var vp = this.HttpContext.Request.ApplicationPath == "/"
                         ? string.Empty
                         : string.Format("{0}/", this.HttpContext.Request.ApplicationPath);

            return string.Format("url('{0}{1}')", vp, url);
        }

        /// <summary>
        /// Gets the actual file content and processes it based on the mimeType. CSS/Less files need
        /// to be processed and have their URLs corrected.
        /// </summary>
        /// <param name="storeItem">
        /// The store item.
        /// </param>
        /// <param name="mimeType">
        /// The mine type.
        /// </param>
        /// <returns>
        /// The file content
        /// </returns>
        private string GetFileContent(IStoreItem storeItem, MimeTypes mimeType)
        {
            var path = this.Server.MapPath(storeItem.File);

            var f = new FileInfo(path);

            if (!f.Exists || f.DirectoryName == null)
            {
                return JavascriptAlert(string.Format("The file or directory does not exist ({0})", path));
            }

            var part = System.IO.File.ReadAllText(path);

            if (string.IsNullOrEmpty(part))
            {
                return JavascriptAlert(string.Format("There is no text for the part ({0})", path));
            }

            try
            {
                if (mimeType == MimeTypes.Css && storeItem.Less)
                {
                    Directory.SetCurrentDirectory(f.DirectoryName);
                    part = Less.Parse(part);

                    if (string.IsNullOrEmpty(part))
                    {
                        return JavascriptAlert(string.Format("The Less result is empty ({0})", storeItem.File));
                    }
                }
            }
            catch (Exception e)
            {
                return
                    JavascriptAlert(
                        string.Format(
                            "The file '{0}' has generated the following message: {1}",
                            storeItem.File,
                            e.Message));
            }

            if (mimeType == MimeTypes.Css)
            {
                var basePath = storeItem.File.Replace("~", string.Empty);
                basePath = basePath.Contains('/') ? basePath.Substring(0, basePath.LastIndexOf('/')) : basePath;

                part = Regex.Replace(
                    part,
                    @"url[\s]*\([\s]*(?<url>[^\)]*)\)",
                    match => this.Matcher(match, basePath));

                if (string.IsNullOrEmpty(part))
                {
                    return JavascriptAlert(string.Format("The Less regex is empty ({0})", storeItem.File));
                }
            }

            return part;
        }

        /// <summary>
        /// Gets the content and processes it based on the mimeType. CSS/Less files need
        /// to be processed and have their URLs corrected.
        /// </summary>
        /// <param name="storeItem">
        /// The store item.
        /// </param>
        /// <param name="mineType">
        /// The mine type.
        /// </param>
        /// <returns>
        /// The content
        /// </returns>
        private string GetInlineContent(IStoreItem storeItem, MimeTypes mineType)
        {
            var path = string.IsNullOrEmpty(storeItem.File) ? null : this.Server.MapPath(storeItem.File);

            var f = string.IsNullOrEmpty(path) ? null : new FileInfo(path);

            if (f != null && (!f.Exists || f.DirectoryName == null))
            {
                return null;
            }

            var part = storeItem.Content;

            try
            {
                if (mineType == MimeTypes.Css && storeItem.Less)
                {
                    if (f != null)
                    {
                        Directory.SetCurrentDirectory(f.DirectoryName);
                    }

                    part = Less.Parse(part);
                }
            }
            catch (Exception e)
            {
                return
                    JavascriptAlert(
                        string.Format(
                            "The file '{0}' has generated the following message: {1}",
                            storeItem.File,
                            e.Message));
            }

            if (mineType == MimeTypes.Css && !string.IsNullOrEmpty(storeItem.File))
            {
                var basePath = storeItem.File.Replace("~", string.Empty);
                basePath = basePath.Contains('/') ? basePath.Substring(0, basePath.LastIndexOf('/')) : basePath;

                part = Regex.Replace(
                    part,
                    @"url[\s]*\([\s]*(?<url>[^\)]*)\)",
                    match => this.Matcher(match, basePath));
            }

            return part;
        }
        
        /// <summary>
        /// Concatenates the supplied store item file contents into one output. 
        /// </summary>
        /// <param name="storeItems">
        /// The store items.
        /// </param>
        /// <param name="mimeType">
        /// The mime type.
        /// </param>
        /// <returns>
        /// The concatenated result
        /// </returns>
        private ContentResult GetContent(IEnumerable<IStoreItem> storeItems, MimeTypes mimeType)
        {
            var content = new StringBuilder();
            var crucherCompression = Conversion.ConvertToBoolean(
                ConfigurationManager.AppSettings["CrucherCompression"], false);

            var minifier = new Minifier();
            var codeSettings = new CodeSettings { PreserveImportantComments = false };
            var cssSettings = new CssSettings { CommentMode = CssComment.None };

            foreach (var si in storeItems.OrderBy(si => si.Order))
            {
                var part = string.Empty;

                switch (si.Reference)
                {
                    case ReferenceOptions.File:
                        part = this.GetFileContent(si, mimeType);
                        break;

                    case ReferenceOptions.Inline:
                        part = this.GetInlineContent(si, mimeType);
                        break;
                }

                if (crucherCompression)
                {
                    switch (si.Reference)
                    {
                        case ReferenceOptions.File:
                            content.Append(part);
                            break;

                        case ReferenceOptions.Inline:
                            content.Append(part);
                            break;
                    }
                }
                else
                {
                    switch (si.Reference)
                    {
                        case ReferenceOptions.File:

                            content.Append(
                                string.Format(
                                    "\n/* ====================== File: {0} ====================== */\n{1}\n", si.File, part));
                            break;

                        case ReferenceOptions.Inline:
                            content.Append(
                                string.Format(
                                    "\n/* ====================== {0} ====================== */\n{1}\n",
                                    string.IsNullOrEmpty(si.Name) ? "Inline" : "Inline: " + si.Name,
                                    part));
                            break;
                    }
                }
            }

            if (crucherCompression)
            {
                switch (mimeType)
                {
                    case MimeTypes.Js:
                        return this.Content(minifier.MinifyJavaScript(content.ToString(), codeSettings), "text/javascript");
                        
                    case MimeTypes.Css:
                        return this.Content(minifier.MinifyStyleSheet(content.ToString(), cssSettings), "text/css");
                }
            }
            else
            {
                switch (mimeType)
                {
                    case MimeTypes.Js:
                        return this.Content(content.ToString(), "text/javascript");
                    case MimeTypes.Css:
                        return this.Content(content.ToString(), "text/css");
                }
            }
            
            return this.Content(string.Empty);
        }
    }
}
