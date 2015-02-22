//--------------------------------------------------------------------------------------
// <copyright file="BootstrapController.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Bootstrap.Controllers
{
    using System.Web.Mvc;

    using Trooper.Properties;

    /// <summary>
    /// The bootstrap controller. Currently supplied the icons for the bootstrap CSS library.
    /// </summary>
    public class BootstrapController : Controller
    {   
        /// <summary>
        /// Provides the glyphicons_halflings.png image
        /// </summary>
        /// <returns>
        /// The <see cref="FileResult"/>.
        /// </returns>
        public FileResult GetGhrEot()
        {
            return new FileContentResult(Resources.GlyphiconsHalflingsRegularEot, "application/vnd.ms-fontobject");
        }

        /// <summary>
        /// Provides the glyphicons_halflings.png image
        /// </summary>
        /// <returns>
        /// The <see cref="FileResult"/>.
        /// </returns>
        public ContentResult GetGhrSvg()
        {
            return this.Content(Resources.GlyphiconsHalflingsReguarSvg, "image/svg+xml");
        }

        /// <summary>
        /// Provides the glyphicons_halflings.png image
        /// </summary>
        /// <returns>
        /// The <see cref="FileResult"/>.
        /// </returns>
        public FileResult GetGhrTtf()
        {
            return new FileContentResult(Resources.GlyphiconsHalflingsRegularTtf, "application/x-font-ttf");
        }

        /// <summary>
        /// Provides the glyphicons_halflings.png image
        /// </summary>
        /// <returns>
        /// The <see cref="FileResult"/>.
        /// </returns>
        public FileResult GetGhrWoff()
        {
            return new FileContentResult(Resources.GlyphiconsHalflingsRegularWoff, "application/font-woff");
        }

        /// <summary>
        /// Provides the glyphicons_halflings.png image
        /// </summary>
        /// <returns>
        /// The <see cref="FileResult"/>.
        /// </returns>
        public FileResult GetGhrWoff2()
        {
            return new FileContentResult(Resources.GlyphiconsHalflingsRegularWoff2, "application/font-woff");
        }

        /// <summary>
        /// Provides the Bootstrap.css.map image
        /// </summary>
        /// <returns>
        /// The <see cref="FileResult"/>.
        /// </returns>
        public ContentResult GetBootstrapCssMap()
        {
            return this.Content(Resources.bootstrap_css_map, "application/text");
        }
    }
}