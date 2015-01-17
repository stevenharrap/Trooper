//--------------------------------------------------------------------------------------
// <copyright file="SimpleVirtualPath.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Common.VirtualPath
{
    /// <summary>
    /// This implements a simple version of VirtualPath for use in simple scenarios
    /// such as providing Resources. Not for fancy DB CMS situations.
    /// </summary>
    public class SimpleVirtualPath : IVirtualPath
    {
        /// <summary>
        /// Gets or sets the path that will result in the Resource being served
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the content from the Resource.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// The is the provided path a match for this Path
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsPath(string virtualPath)
        {
            return virtualPath == this.GetAppSafePath();
        }

        /// <summary>
        /// Get a version of the path that includes any application path.
        /// </summary>
        /// <returns>
        /// The path with the application path included.
        /// </returns>
        public string GetAppSafePath()
        {
            return VirtualPathFactory.MakeAppSafePath(this.Path);
        }
        
        /// <summary>
        /// The get the content for the virtual file base on the path provided. It is assumed that
        /// IsPath agrees that the supplied virtual is a match for this path.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        /// <returns>
        /// Returns array of byte that represents the content of the virtual view
        /// </returns>
        public byte[] GetContent(string virtualPath)
        {
            return this.Content;
        }
    }
}
