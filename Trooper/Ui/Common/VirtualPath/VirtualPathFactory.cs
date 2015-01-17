//--------------------------------------------------------------------------------------
// <copyright file="VirtualPathFactory.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Common.VirtualPath
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;

    /// <summary>
    /// The Virtual Path Factory allows you to use MVC Views that don't exist in your file system. Typically
    /// these might be embedded Resources or data pulled from a database record. In a more dynamic sense you
    /// may have one dynamic path which will return variable content based on the supplied path.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// /* in your application somewhere  */
    /// public class MyVirtualPathProvidor : VirtualPathFactory
    /// {
    ///     public static SimpleVirtualPath List = new SimpleVirtualPath
    ///                                          {
    ///                                              Path = "/A.Unique.String/List.cshtml",
    ///                                              Content = Resources.List_cshtml
    ///                                           };
    ///     public static SimpleVirtualPath Selector = new SimpleVirtualPath
    ///                                              {
    ///                                                  Path = "/A.Unique.String/Selector.cshtml",
    ///                                                  Content = Resources.Selector_cshtml
    ///                                              };
    ///     public MyVirtualPathProvidor()
    ///     {
    ///         this.Paths = new List<IVirtualPath> { List, Selector };
    ///     }
    /// }
    /// /* 
    ///     In Application_Start() 
    /// */
    /// VirtualPathFactory.RegisterVirtualPathsUtility(new MyVirtualPathProvidor());
    /// /* 
    ///     In a view 
    /// */
    /// @Html.Partial("/A.Unique.String/Selector.cshtml", yourmodel);
    /// ]]>
    /// </example>
    public class VirtualPathFactory : VirtualPathProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPathFactory"/> class.
        /// </summary>
        public VirtualPathFactory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPathFactory"/> class.
        /// </summary>
        /// <param name="paths">
        /// The paths the VirtualPathFactory should listen for.
        /// </param>
        public VirtualPathFactory(List<IVirtualPath> paths)
        {
            this.Paths = paths;
        }
        
        /// <summary>
        /// Gets or sets the paths the VirtualPathFactory should listen for. 
        /// </summary>
        public List<IVirtualPath> Paths { get; set; }

        /// <summary>
        /// Registers this Factory into your application.
        /// </summary>
        /// <param name="vpp">
        /// The VirtualPathFactory.
        /// </param>
        public static void RegisterVirtualPathsUtility(VirtualPathFactory vpp)
        {
            HostingEnvironment.RegisterVirtualPathProvider(vpp);
        }

        /// <summary>
        /// Makes the virtual path application path safe. 
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string MakeAppSafePath(string virtualPath)
        {
            var ap = HttpContext.Current.Request.ApplicationPath;

            return string.Format("{0}{1}", ap == "/" ? string.Empty : ap, virtualPath);
        }

        /// <summary>
        /// Does the virtual path exist. If not hand onto the base system to find a physical file.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool FileExists(string virtualPath)
        {
            return this.HasPage(virtualPath) || base.FileExists(virtualPath);
        }

        /// <summary>
        /// The get file content for the virtual path. If a virtual file can't be found the request will be passed onto the base system.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        /// <returns>
        /// The <see cref="VirtualFile"/>.
        /// </returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            var vp = this.GetPage(virtualPath);
            
            return vp == null ? base.GetFile(virtualPath) : new VirtualPathFactoryFile(virtualPath, vp.GetContent(virtualPath));
        }

        /// <summary>
        /// Creates a cache dependency based on the specified virtual paths.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.Caching.CacheDependency"/> object for the specified virtual resources.
        /// </returns>
        /// <param name="virtualPath">The path to the primary virtual resource.</param>
        /// <param name="virtualPathDependencies">An array of paths to other resources required by the primary virtual resource.</param>
        /// <param name="utcStart">The UTC time at which the virtual resources were read.</param>
        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return this.HasPage(virtualPath) ? null : base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        /// <summary>
        /// Does the factory have a page that matches the virtual path request
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool HasPage(string virtualPath)
        {
            return this.Paths.Any(p => p.IsPath(virtualPath));
        }

        /// <summary>
        /// Get the first page that matches the virtual path request
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        /// <returns>
        /// The <see cref="IVirtualPath"/>.
        /// </returns>
        private IVirtualPath GetPage(string virtualPath)
        {
            return this.Paths.FirstOrDefault(p => p.IsPath(virtualPath));
        }
    }
}