//--------------------------------------------------------------------------------------
// <copyright file="StdThemingVirtualPathProvider.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Ui.StdTemplate
{
    using System.Collections.Generic;

    using Trooper.Properties;
    using Trooper.Ui.Common.VirtualPath;

    /// <summary>
    /// The people selector virtual path provider.
    /// </summary>
    internal class StdThemingVirtualPathProvider : VirtualPathFactory
    {
        /// <summary>
        /// The selector view.
        /// </summary>
        public static readonly SimpleVirtualPath LayoutView = new SimpleVirtualPath
        {
            Path = "/Trooper.App.Ui.StdTemplate/LayoutView.cshtml",
            Content = Resources.LayoutView_cshtml
        };

        /// <summary>
        /// The selector view.
        /// </summary>
        public static readonly SimpleVirtualPath SearchBarView = new SimpleVirtualPath
        {
            Path = "/Trooper.App.Ui.StdTemplate/SearchBar.cshtml",
            Content = Resources.SearchBarView_cshtml
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="StdThemingVirtualPathProvider"/> class.
        /// </summary>
        public StdThemingVirtualPathProvider()
        {
            this.Paths = new List<IVirtualPath> { LayoutView, SearchBarView };
        }
    }
}