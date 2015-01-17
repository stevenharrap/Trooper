//--------------------------------------------------------------------------------------
// <copyright file="VirtualPathFactoryFile.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Common.VirtualPath
{
    using System.IO;
    using System.Web.Hosting;

    /// <summary>
    /// Provide file content derived from virtual sources.
    /// </summary>
    public class VirtualPathFactoryFile : VirtualFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPathFactoryFile"/> class.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        /// <param name="content">
        /// The content of the file.
        /// </param>
        public VirtualPathFactoryFile(string virtualPath, byte[] content)
            : base(virtualPath)
        {
            this.Content = content;
        }

        /// <summary>
        /// Gets or sets the content of the virtual file.
        /// </summary>
        private byte[] Content { get; set; }

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public override Stream Open()
        {
            return new MemoryStream(this.Content);
        }
    }
}
