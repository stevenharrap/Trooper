//--------------------------------------------------------------------------------------
// <copyright file="StdTheming.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Ui.StdTemplate
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.WebPages;

    using Trooper.App.Ui.StdTemplate.Controllers;
    using Trooper.App.Ui.StdTemplate.Models;
    using Trooper.Properties;
    using Trooper.Ui.Mvc.Cruncher;

    /// <summary>
    /// Applications tend follow a similar layout, color and font scheme. This project uses
    /// Cruncher to inject CSS into the page which provides uniform fonts and eventually colors and
    /// other CSS elements. Calling the Standard method in this class will inject the CSS required
    /// into Cruncher.
    /// </summary>
    /// <remarks>
    /// This project could be targeted to return just the font types appropriate to the calling client.
    /// </remarks>
    public class StdTheming
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StdTheming"/> class.
        /// Injects the CSS required into Cruncher. Call from somewhere in your page.
        /// </summary>
        /// <param name="htmlHelper">
        /// The html helper.
        /// </param>
        public StdTheming(HtmlHelper htmlHelper)
        {
            var cruncher = new Cruncher(htmlHelper);

            if (cruncher.HeaderCss().HasItem("StdTheming"))
            {
                return;
            }

            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var css = Resources.fonts_less;
            
            var regularEot = urlHelper.Action(
                "GetFont",
                "StdTheming",
                new
                    {
                        font = StdThemingController.Fonts.ProximaNovaRegular,
                        fontformat = StdThemingController.FontFormats.Eot
                    });
            var regularWoff = urlHelper.Action(
                "GetFont",
                "StdTheming",
                new
                    {
                        font = StdThemingController.Fonts.ProximaNovaRegular,
                        fontformat = StdThemingController.FontFormats.Woff
                    });
            var regularTtf = urlHelper.Action(
                "GetFont",
                "StdTheming",
                new
                    {
                        font = StdThemingController.Fonts.ProximaNovaRegular,
                        fontformat = StdThemingController.FontFormats.Truetype
                    });
            var regularSvg = urlHelper.Action(
                "GetFont",
                "StdTheming",
                new
                    {
                        font = StdThemingController.Fonts.ProximaNovaRegular,
                        fontformat = StdThemingController.FontFormats.Svg
                    });

            var mediumEot = urlHelper.Action(
                "GetFont",
                "StdTheming",
                new
                    {
                        font = StdThemingController.Fonts.ProximaNovaMedium,
                        fontformat = StdThemingController.FontFormats.Eot
                    });
            var mediumWoff = urlHelper.Action(
                "GetFont",
                "StdTheming",
                new
                    {
                        font = StdThemingController.Fonts.ProximaNovaMedium,
                        fontformat = StdThemingController.FontFormats.Woff
                    });
            var mediumTtf = urlHelper.Action(
                "GetFont",
                "StdTheming",
                new
                    {
                        font = StdThemingController.Fonts.ProximaNovaMedium,
                        fontformat = StdThemingController.FontFormats.Truetype
                    });
            var mediumSvg = urlHelper.Action(
                "GetFont",
                "StdTheming",
                new
                    {
                        font = StdThemingController.Fonts.ProximaNovaMedium,
                        fontformat = StdThemingController.FontFormats.Svg
                    });

            var boldEot = urlHelper.Action(
                "GetFont",
                "StdTheming",
                new
                    {
                        font = StdThemingController.Fonts.ProximaNovaBold,
                        fontformat = StdThemingController.FontFormats.Eot
                    });

            var boldWoff = urlHelper.Action(
                "GetFont",
                "StdTheming",
                new
                    {
                        font = StdThemingController.Fonts.ProximaNovaBold,
                        fontformat = StdThemingController.FontFormats.Woff
                    });

            var boldTtf = urlHelper.Action(
                "GetFont",
                "StdTheming",
                new
                    {
                        font = StdThemingController.Fonts.ProximaNovaBold,
                        fontformat = StdThemingController.FontFormats.Truetype
                    });

            var boldSvg = urlHelper.Action(
                "GetFont",
                "StdTheming",
                new
                    {
                        font = StdThemingController.Fonts.ProximaNovaBold,
                        fontformat = StdThemingController.FontFormats.Svg
                    });

            css = css.Replace("Regular.eot", regularEot);
            css = css.Replace("Regular.woff", regularWoff);
            css = css.Replace("Regular.ttf", regularTtf);
            css = css.Replace("Regular.svg", regularSvg);

            css = css.Replace("Medium.eot", mediumEot);
            css = css.Replace("Medium.woff", mediumWoff);
            css = css.Replace("Medium.ttf", mediumTtf);
            css = css.Replace("Medium.svg", mediumSvg);

            css = css.Replace("Bold.eot", boldEot);
            css = css.Replace("Bold.woff", boldWoff);
            css = css.Replace("Bold.ttf", boldTtf);
            css = css.Replace("Bold.svg", boldSvg);

            cruncher.HeaderCss().AddInline(css, name: "StdTheming", order: StoreItem.OrderOptions.First, less: true);
        }

        /// <summary>
        /// The initiates the default Standard theming by instantiating the class.
        /// </summary>
        /// <param name="htmlHelper">
        /// The html helper.
        /// </param>
        /// <returns>
        /// The <see cref="StdTheming"/>.
        /// </returns>
        public static StdTheming Init(HtmlHelper htmlHelper)
        {
            return new StdTheming(htmlHelper);
        }

        /// <summary>
        /// Makes a search bar that goes below the black title bar and above the
        /// main content. The layout allows you to put your controls in the 
        /// inputs area and the search and clear buttons are provided.
        /// </summary>
        /// <param name="htmlHelper">The HtmlHelper from your view</param>
        /// <param name="model">The model from your view</param>
        /// <param name="html">The Html that you want to place in the inputs area of
        /// the search bar.</param>
        /// <param name="searchBarModel">Optionally include the SearchBarModel
        /// to provide more options.</param>
        /// <returns>Returns a MvcHtmlString</returns>
        public static MvcHtmlString MakeSearchBar(
            HtmlHelper htmlHelper,
            StdLayoutModel model,
            Func<object, HelperResult> html,
            SearchBarModel searchBarModel = null)
        {
            var sbm = searchBarModel ?? new SearchBarModel();

            sbm.StdLayoutModel = model;
            sbm.Html = html;

            return htmlHelper.Partial(StdThemingVirtualPathProvider.SearchBarView.GetAppSafePath(), sbm);
        }
    }
}
