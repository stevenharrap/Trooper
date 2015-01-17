//--------------------------------------------------------------------------------------
// <copyright file="IVirtualPath.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Common.VirtualPath
{
    /// <summary>
    /// The VirtualPath interface. This interface provides the structure for determining
    /// if a supplied virtual path is a match for this virtual path. 
    /// </summary>
    public interface IVirtualPath
    {
        /// <summary>
        /// Is the supplied virtual path a match for this virtual path. If the context is a
        /// CMS then the supplied path might include parameters. In this case you would only
        /// be looking at the base of the virtual path. GetPath would concern itself in regard
        /// to the parameters.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsPath(string virtualPath);

        /// <summary>
        /// Get a version of the path that includes any application path.
        /// </summary>
        /// <returns>
        /// The path with the application path included.
        /// </returns>
        string GetAppSafePath();

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
        byte[] GetContent(string virtualPath);
    }
}
