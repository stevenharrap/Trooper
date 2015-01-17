//--------------------------------------------------------------------------------------
// <copyright file="UploadModel.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Bootstrap.Models
{
    using System;
    using System.IO;
    using System.Web.Helpers;
    using System.Web.Hosting;

    using Trooper.Ui.Mvc.Bootstrap;
    using Trooper.Utility;

    /// <summary>
    /// This model is used by the Upload control on the page and persists the details of the uploaded or
    /// existing file between posts.
    /// </summary>
    public class UploadModel
    {
        private bool currentDocumentDeleted;

        private string currentDocumentUrl;

        private string currentDocumentFilename;

        private string persistedFilename;

        private Guid persistedId;

        private bool Persisted { get; set; }
        
        public string PersistedData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current document is to be deleted.
        /// </summary>
        public bool CurrentDocumentDeleted
        {
            get
            {
                if (!this.Persisted)
                {
                    this.Persist();
                }

                return this.currentDocumentDeleted;
            }

            set
            {
                if (!this.Persisted)
                {
                    this.Persist();
                }

                this.currentDocumentDeleted = value;
            }
        }

        /// <summary>
        /// Gets or sets the current document URL if the file already exists.
        /// </summary>
        public string CurrentDocumentUrl
        {
            get
            {
                if (!this.Persisted)
                {
                    this.Persist();
                }

                return this.currentDocumentUrl;
            }

            set
            {
                if (!this.Persisted)
                {
                    this.Persist();
                }

                this.currentDocumentUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the current document name if the file already exists.
        /// </summary>
        public string CurrentDocumentFilename
        {
            get
            {
                if (!this.Persisted)
                {
                    this.Persist();
                }

                return this.currentDocumentFilename;
            }

            set
            {
                if (!this.Persisted)
                {
                    this.Persist();
                }

                this.currentDocumentFilename = value;
            }
        }

        /// <summary>
        /// Gets or sets the persist name of the uploaded file 
        /// </summary>
        public string PersistedFilename
        {
            get
            {
                if (!this.Persisted)
                {
                    this.Persist();
                }

                return this.persistedFilename;
            }

            set
            {
                if (!this.Persisted)
                {
                    this.Persist();
                }

                this.persistedFilename = value;
            }
        }

        /// <summary>
        /// Gets or sets the persist id of the control.
        /// </summary>
        public Guid PersistedId
        {
            get
            {
                if (!this.Persisted)
                {
                    this.Persist();
                }

                return this.persistedId;
            }

            set
            {
                if (!this.Persisted)
                {
                    this.Persist();
                }

                this.persistedId = value;
            }
        }

        /// <summary>
        /// Gets the name of the uploaded file (no extension and no directory).
        /// </summary>
        public string UploadedFilenameOnly
        {
            get
            {
                if (string.IsNullOrEmpty(this.PersistedFilename))
                {
                    return string.Empty;
                }

                return Path.GetFileNameWithoutExtension(this.PersistedFilename);
            }
        }

        /// <summary>
        /// Gets the extension of the uploaded file.
        /// </summary>
        public string UploadedExtension
        {
            get
            {
                if (string.IsNullOrEmpty(this.PersistedFilename))
                {
                    return string.Empty;
                }

                return Path.GetExtension(this.PersistedFilename);
            }
        }

        /// <summary>
        /// Gets a value indicating whether are file has been uploaded.
        /// </summary>
        public bool IsFileUploaded
        {
            get
            {
                if (this.PersistedId == Guid.Empty)
                {
                    return false;
                }
                
                return File.Exists(this.TempFilePath);
            }
        }

        /// <summary>
        /// Gets the uploaded file as a stream.
        /// </summary>
        public FileStream UploadedFileAsStream
        {
            get
            {
                if (!this.IsFileUploaded)
                {
                    return null;
                }

                var path = this.TempFilePath;
                
                return File.OpenRead(path);
            }
        }

        /// <summary>
        /// Gets the uploaded file as bytes.
        /// </summary>
        public byte[] UploadedFileAsBytes
        {
            get
            {
                if (!this.IsFileUploaded)
                {
                    return null;
                }

                var path = this.TempFilePath;

                return File.ReadAllBytes(path);
            }
        }

        /// <summary>
        /// Gets the temp file path.
        /// </summary>
        private string TempFilePath
        {
            get
            {
                return HostingEnvironment.MapPath(string.Format(@"{0}\{1}", UploadHelper.PersistedDocFolder, this.PersistedId));
            }
        }

        private void Persist()
        {
            this.Persisted = true;

            if (this.PersistedData == null)
            {
                return;
            }

            var data = Json.Decode(this.PersistedData);

            this.currentDocumentDeleted = Conversion.ConvertToBoolean(data.CurrentDocumentDeleted, false);
            this.currentDocumentUrl = Conversion.ConvertToString(data.CurrentDocumentUrl, string.Empty);
            this.currentDocumentFilename = Conversion.ConvertToString(data.CurrentDocumentFilename, string.Empty);
            this.persistedFilename = Conversion.ConvertToString(data.PersistedFilename, string.Empty);
            this.persistedId = Conversion.ConvertToGuid(data.PersistedId, Guid.Empty);
        }
    }
}
