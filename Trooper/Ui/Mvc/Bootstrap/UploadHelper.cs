//--------------------------------------------------------------------------------------
// <copyright file="UploadInit.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Bootstrap
{
    using System.IO;
    using System.Web.Hosting;

    /// <summary>
    /// Initiates the Upload control for use in your code.
    /// </summary>
    public class UploadHelper
    {
        /// <summary>
        /// The folder location where uploaded documents are held while the form submits due to other
        /// user requests.
        /// </summary>
        public const string PersistedDocFolder = "~/AppData/PersistedDocs";

        /// <summary>
        /// Clears out any existing uploads. Do this in App_Start method.
        /// </summary>
        public static void ClearUploads()
        {
            var path = HostingEnvironment.MapPath(PersistedDocFolder);

            if (path != null && Directory.Exists(path))
            {
                foreach (var f in new DirectoryInfo(path).GetFiles())
                {
                    f.Delete();
                }
            }
        }
    }
}
