//--------------------------------------------------------------------------------------
// <copyright file="StdThemingController.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Ui.StdTemplate.Controllers
{
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web.Mvc;

    using Trooper.Properties;

    /// <summary>
    /// Trooper Applications tend follow a similar layout, color and font scheme. This project uses
    /// Cruncher to inject CSS into the page which provides uniform fonts and eventually colors and
    /// other CSS elements. This controller returns requests for font files from the CSS font rules.
    /// The controller methods take a combination of the required font and font format.
    /// </summary>
    /// <remarks>
    /// This project could be targeted to return just the font types appropriate to the calling client.
    /// </remarks>
    public class StdThemingController : Controller
    {
        /// <summary>
        /// The fonts faces that can be served up
        /// </summary>
        public enum Fonts
        {
            /// <summary>
            /// The proxima nova regular font face.
            /// </summary>
            ProximaNovaRegular,

            /// <summary>
            /// The proxima nova medium font face.
            /// </summary>
            ProximaNovaMedium,

            /// <summary>
            /// The proxima nova bold font face.
            /// </summary>
            ProximaNovaBold
        }

        /// <summary>
        /// The font formats available.
        /// </summary>
        public enum FontFormats
        {
            /// <summary>
            /// The EOT font type.
            /// </summary>
            Eot,

            /// <summary>
            /// The WOFF font type.
            /// </summary>
            Woff,

            /// <summary>
            /// The TrueType font type.
            /// </summary>
            Truetype,

            /// <summary>
            /// The SVG font type.
            /// </summary>
            Svg
        }

        /// <summary>
        /// Gets the logo
        /// </summary>
        /// <returns>
        /// The logo as a binary source.
        /// </returns>
        [HttpGet]
        [OutputCache(Duration = 3600)]
        public FileStreamResult GetLogo()
        {
            var ms = new MemoryStream();
            Resources.trooper_png.Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(ms, "image/png");
        }

        /// <summary>
        /// Gets the font file based on the font face and font format type
        /// </summary>
        /// <param name="font">
        /// The font face.
        /// </param>
        /// <param name="fontformat">
        /// The font format.
        /// </param>
        /// <returns>
        /// The <see cref="FileResult"/>.
        /// </returns>
        [HttpGet]
        [OutputCache(Duration = 3600)]
        public FileResult GetFont(Fonts? font, FontFormats? fontformat)
        {
            const string EotMimetype = "application/vnd.ms-fontobject";
            const string WotMimetype = "application/font-woff";
            const string TtfMimetype = "application/x-font-ttf";
            const string SvgMimetype = "image/svg+xml";

            switch (font)
            {
                case Fonts.ProximaNovaRegular:
                    switch (fontformat)
                    {
                        case FontFormats.Eot:
                            return new FileContentResult(Resources.ProximaNovaSoft_Regular_webfont_eot, EotMimetype);
                        case FontFormats.Woff:
                            return new FileContentResult(Resources.ProximaNovaSoft_Regular_webfont_woff, WotMimetype);
                        case FontFormats.Truetype:
                            return new FileContentResult(Resources.ProximaNovaSoft_Regular_webfont_ttf, TtfMimetype);
                        case FontFormats.Svg:
                            return new FileContentResult(Resources.ProximaNovaSoft_Regular_webfont_svg, SvgMimetype);
                        default:
                            return null;
                    }

                case Fonts.ProximaNovaMedium:
                    switch (fontformat)
                    {
                        case FontFormats.Eot:
                            return new FileContentResult(Resources.ProximaNovaSoft_Medium_webfont_eot, EotMimetype);
                        case FontFormats.Woff:
                            return new FileContentResult(Resources.ProximaNovaSoft_Medium_webfont_woff, WotMimetype);
                        case FontFormats.Truetype:
                            return new FileContentResult(Resources.ProximaNovaSoft_Medium_webfont_ttf, TtfMimetype);
                        case FontFormats.Svg:
                            return new FileContentResult(Resources.ProximaNovaSoft_Medium_webfont_svg, SvgMimetype);
                        default:
                            return null;
                    }

                case Fonts.ProximaNovaBold:
                    switch (fontformat)
                    {
                        case FontFormats.Eot:
                            return new FileContentResult(Resources.ProximaNovaSoft_Bold_webfont_eot, EotMimetype);
                        case FontFormats.Woff:
                            return new FileContentResult(Resources.ProximaNovaSoft_Bold_webfont_woff, WotMimetype);
                        case FontFormats.Truetype:
                            return new FileContentResult(Resources.ProximaNovaSoft_Bold_webfont_ttf, TtfMimetype);
                        case FontFormats.Svg:
                            return new FileContentResult(Resources.ProximaNovaSoft_Bold_webfont_svg, SvgMimetype);
                        default:
                            return null;
                    }

                default:
                    return null;
            }
        }
    }
}