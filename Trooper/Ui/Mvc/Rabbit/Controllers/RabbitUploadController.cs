//--------------------------------------------------------------------------------------
// <copyright file="BsUploadController.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Rabbit.Controllers
{
    using System;
    using System.IO;
    using System.Web.Hosting;
    using System.Web.Mvc;

    using Trooper.Properties;
    using Trooper.Ui.Mvc.Rabbit;
    using Trooper.Ui.Mvc.Rabbit.Models;
    using Trooper.Ui.Mvc.Utility;

    /// <summary>
    /// When an upload control is created on the page it will write an IFrame and that URL
    /// will call this controller. In response this controller will create a hidden form uploading the file
    /// and that form will also target this controller as well. When a file is uploaded the form response
    /// will fire JavaScript in the containing page to indicate that the upload is complete.
    /// </summary>
    public class RabbitUploadController : Controller
    {
        /// <summary>
        /// The default state. No file uploaded
        /// </summary>
        /// <param name="id">
        /// The input id of the control on the page.
        /// </param>
        /// <returns>
        /// The <see cref="ContentResult"/>.
        /// </returns>
        public ContentResult OpenIframe(string id)
        {
            var m = new FrameModel { Id = id };

            return this.UploadIframe(m);
        }

        /// <summary>
        /// The upload IFrame after a post from the client. An upload may have just occurred
        /// or the form may be posting back for other reasons.
        /// </summary>
        /// <param name="m">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="ContentResult"/>.
        /// </returns>
        [HttpPost]
        public ContentResult UploadIframe(FrameModel m)
        {
            var urlHelper = new UrlHelper(this.HttpContext.Request.RequestContext);
            var action = urlHelper.Action("FileUploaded", "RabbitUpload");

            return new ContentResult
                {
                    Content =
                        "<html>\n<head>\n<script type=\"text/javascript\">\n" + Resources.jquery_min_js + "\n</script>\n"
                        + "<script type=\"text/javascript\">\n"
                        + "$(document).ready(function(){$('#UploadedFile').change(function(){$('form').submit();});});\n"
                        + "$(document).ready(function(){parent.window.trooper.ui.registry.getUpload('" + m.Id + "').iFrameLoad();});\n"
                        + "</script></head>\n<body>\n" + "<form action=\"" + action
                        + "\" method=\"post\" enctype=\"multipart/form-data\">\n"
                        + "<input type=\"textbox\" id=\"DisplayFilename\" name=\"DisplayFilename\" value=\"" + m.DisplayFilename
                        + "\" />\n"
                        + "<input type=\"textbox\" id=\"PersistedId\" name=\"PersistedId\" value=\"" + m.PersistedId
                        + "\" />\n"
                        + "<input type=\"textbox\" id=\"Id\" name=\"Id\" value=\"" + m.Id
                        + "\" />\n"
                        + "<input type=\"file\" id=\"UploadedFile\" name=\"UploadedFile\" />\n</form>\n"
                        + "</body>\n</html>"
                };
        }

        /// <summary>
        /// The user has selected a file and the IFrame form has automatically submitted.
        /// The file is saved to a temporary location and the model is updated with details of the file.
        /// </summary>
        /// <param name="m">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="ContentResult"/>.
        /// </returns>
        [HttpPost]
        public ContentResult FileUploaded(FrameModel m)
        {
            var upload = this.Request.Files["UploadedFile"];

            if (upload == null)
            {
                return this.UploadIframe(m);
            }

            var folder = HostingEnvironment.MapPath(RabbitHelper.PersistedDocFolder);

            if (folder == null)
            {
                return this.UploadIframe(m);
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (m.PersistedId == Guid.Empty)
            {
                m.PersistedId = Guid.NewGuid();
            }

            m.DisplayFilename = Path.GetFileName(upload.FileName);

            upload.SaveAs(string.Format(@"{0}\{1}", folder, m.PersistedId));

            return this.UploadIframe(m);
        }
    }
}
