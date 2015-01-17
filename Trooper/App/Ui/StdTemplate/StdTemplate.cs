//--------------------------------------------------------------------------------------
// <copyright file="StdTemplate.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Ui.StdTemplate
{
    using System;
    using System.Web.WebPages;

    using Trooper.Ui.Common.VirtualPath;

    /// <summary>
    /// Provides the methods and initiate methods to integrate the Standard template into your project.
    /// </summary>
    public class StdTemplate
    {
        /// <summary>
        /// A default object for the render extensions
        /// </summary>
        private static readonly object Obj = new object();

        /// <summary>
        /// Provides the correct path the virtual view
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetLayoutViewPath()
        {
            return StdThemingVirtualPathProvider.LayoutView.GetAppSafePath();
        }

        /// <summary>
        /// This will initiate path to the virtual view into your project. See Readme.
        /// </summary>
        public static void Init()
        {
            VirtualPathFactory.RegisterVirtualPathsUtility(new StdThemingVirtualPathProvider());
        }

        /// <summary>
        /// The render section method as an extension for the redefine sections below.
        /// This is all here to help the virtual view work with your application view.
        /// <see cref="http://blogs.msdn.com/b/marcinon/archive/2010/12/15/razor-nested-layouts-and-redefined-sections.aspx"/>
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="sectionName">
        /// The section name.
        /// </param>
        /// <param name="defaultContent">
        /// The default content.
        /// </param>
        /// <returns>
        /// The <see cref="HelperResult"/>.
        /// </returns>
        public static HelperResult RenderSection(
            WebPageBase page,
            string sectionName,
            Func<object, HelperResult> defaultContent)
        {
            return page.IsSectionDefined(sectionName) ? page.RenderSection(sectionName) : defaultContent(Obj);
        }

        /// <summary>
        /// Allows your to redefine a sections content in middle views.
        /// This is all here to help the virtual view work with your application view.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="sectionName">
        /// The section name.
        /// </param>
        /// <returns>
        /// The <see cref="HelperResult"/>.
        /// </returns>
        public static HelperResult RedefineSection(WebPageBase page, string sectionName)
        {
            return RedefineSection(page, sectionName, defaultContent: null);
        }

        /// <summary>
        /// Allows your to redefine a sections content in middle views.
        /// This is all here to help the virtual view work with your application view.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="sectionName">
        /// The section name.
        /// </param>
        /// <param name="defaultContent">
        /// The default content.
        /// </param>
        /// <returns>
        /// The <see cref="HelperResult"/>.
        /// </returns>
        public static HelperResult RedefineSection(
            WebPageBase page,
            string sectionName,
            Func<object, HelperResult> defaultContent)
        {
            if (page.IsSectionDefined(sectionName))
            {
                page.DefineSection(sectionName, () => page.Write(page.RenderSection(sectionName)));
            }
            else if (defaultContent != null)
            {
                page.DefineSection(sectionName, () => page.Write(defaultContent(Obj)));
            }

            return new HelperResult(_ => { });
        }
    }
}