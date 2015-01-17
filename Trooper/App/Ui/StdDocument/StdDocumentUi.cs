//--------------------------------------------------------------------------------------
// <copyright file="StdDocumentUi.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Ui.StdDocument
{
    using System.Web;
    using System.Web.Mvc;

    using Trooper.App.Business.StdDocument.StdModel;
    using Trooper.BusinessOperation.Business;
    using Trooper.BusinessOperation.Interface;
    using Trooper.BusinessOperation.Utility;
    using Trooper.Ui.Mvc.Bootstrap.Models;
    using Trooper.Utility;

    /// <summary>
    /// The standard document UI class for use in Controllers at the presentation layer.
    /// </summary>
    public class StdDocumentUi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StdDocumentUi"/> class.
        /// </summary>
        public StdDocumentUi()
        {
            this.Controller = "Document";
            this.Action = "View";
        }

        /// <summary>
        /// Gets or sets the controller that is used to return documents.
        /// By default this is "Document"
        /// </summary>
        private string Controller { get; set; }

        /// <summary>
        /// Gets or sets the action in the controller that is used to return documents.
        /// By default this is "View"
        /// </summary>
        private string Action { get; set; }

        /// <summary>
        /// Makes a best guess at the most appropriate mime type for file extension
        /// </summary>
        /// <param name="extension">The extension of the file</param>
        /// <returns>The mime type for the extension</returns>
        public static string GetMimeType(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return string.Empty;
            }

            switch (extension.ToLower().Replace(".", string.Empty))
            {
                case "odt":
                    return "application/vnd.oasis.opendocument.text";
                case "doc":
                    return "application/msword";
                case "docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case "wps":
                case "rtf":
                case "pdf":
                case "zip":
                    return string.Format("application/{0}", extension);
                case "png":
                case "gif":
                    return string.Format("image/{0}", extension);
                case "jpeg":
                case "jpg":
                    return "image/jpeg";
                case "txt":
                case "text":
                    return "text/plain";
                default:
                    return "'application/octet-stream";
            }
        }
        
        /// <summary>
        /// Turns an UploadModel into a managed StdDocumentPrp suitable for sending to
        /// a business object.
        /// </summary>
        /// <typeparam name="TDocumentPrp">
        /// The document property class type
        /// </typeparam>
        /// <param name="um">
        /// The UploadModel.
        /// </param>
        /// <param name="existingDocId">
        /// The existing doc id (if updating)
        /// </param>
        /// <returns>
        /// The returns a ManageItem for the Doc.
        /// </returns>
        public ManageItem<TDocumentPrp> PrepareForBo<TDocumentPrp>(UploadModel um, int? existingDocId)
            where TDocumentPrp : class, IStdDocumentPrp, new()
        {
            if (um == null)
            {
                return null;
            }

            if (um.CurrentDocumentDeleted)
            {
                return ManageItemUtility.Delete<TDocumentPrp>();
            }

            if (um.IsFileUploaded)
            {
                return
                    ManageItemUtility.Change(
                        new TDocumentPrp
                        {
                            DocumentId = Conversion.ConvertToInt(existingDocId, 0),
                            Data = um.UploadedFileAsBytes,
                            Filename = um.UploadedFilenameOnly,
                            Extension = um.UploadedExtension
                        });
            }

            return null;
        }

        /// <summary>
        /// Clears the upload model of any existing data and returns a new one.
        /// </summary>
        /// <param name="um">
        /// The upload model.
        /// </param>
        /// <returns>
        /// The <see cref="UploadModel"/>.
        /// </returns>
        public UploadModel Clear(UploadModel um)
        {
            return new UploadModel();
        }

        /// <summary>
        /// Updates an UploadModel for the document on the loading or refreshing of the page.
        /// </summary>
        /// <typeparam name="TSearch">
        /// The search class
        /// </typeparam>
        /// <typeparam name="TStdDocument">
        /// The document class
        /// </typeparam>
        /// <typeparam name="TStdDocumentPrp">
        /// The document property class
        /// </typeparam>
        /// <typeparam name="TStdDocumentKey">
        /// The document key class
        /// </typeparam>
        /// <param name="um">
        /// The UploadModel. Provide your existing upload model if your page is reloading.
        /// </param>
        /// <param name="docId">
        /// The document Id. Can be null if there is no existing document.
        /// </param>
        /// <param name="documentBo">
        /// The document Bo.
        /// </param>
        /// <param name="operationResponse">
        /// The operation Response.
        /// </param>
        /// <returns>
        /// Returns a new or persisted upload model.
        /// </returns>
        public UploadModel PageLoad<TSearch, TStdDocument, TStdDocumentPrp, TStdDocumentKey>(
            UploadModel um,
            int? docId,
            IBusinessR<TSearch, TStdDocument, TStdDocumentPrp, TStdDocumentKey> documentBo,
            IOperationResponse operationResponse)
            where TSearch : class, ISearch, new()
            where TStdDocument : class, TStdDocumentPrp, IStdDocument, new()
            where TStdDocumentPrp : class, TStdDocumentKey, IStdDocumentPrp, new()
            where TStdDocumentKey : class, IStdDocumentKey, IEntityKey<TStdDocumentKey>, new()
        {
            if (um == null)
            {
                um = new UploadModel();
            }

            var dr = docId == null ? null : documentBo.GetByKey(new TStdDocumentKey { DocumentId = (int)docId });

            if (dr == null)
            {
                return um;
            }

            if (!dr.Ok && operationResponse != null)
            {
                MessageUtility.Add(dr, operationResponse);
            }

            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            um.CurrentDocumentFilename = dr.Item.Filename + dr.Item.Extension;
            um.CurrentDocumentUrl = urlHelper.Action(this.Action, this.Controller, new { dr.Item.DocumentId });

            return um;
        }
    }
}
