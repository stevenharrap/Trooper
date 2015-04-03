//--------------------------------------------------------------------------------------
// <copyright file="BootstrapController.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Rabbit.Controllers
{
    using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Resources;
using System.Web.Mvc;
using Trooper.Properties;

    /// <summary>
    /// The bootstrap controller. Currently supplied the icons for the bootstrap CSS library.
    /// </summary>
    public class RabbitController : Controller
    {
        private static Dictionary<string, Func<FileResult>> BinaryResources = new Dictionary<string, Func<FileResult>> 
        {
            { "GlyphiconsHalflingsRegularEot", () => new FileContentResult(Resources.GlyphiconsHalflingsRegularEot, "application/vnd.ms-fontobject")},
            { "GlyphiconsHalflingsRegularTtf", () => new FileContentResult(Resources.GlyphiconsHalflingsRegularTtf, "application/x-font-ttf")},
            { "GlyphiconsHalflingsRegularWoff", () => new FileContentResult(Resources.GlyphiconsHalflingsRegularWoff, "application/font-woff")},
            { "GlyphiconsHalflingsRegularWoff2", () => new FileContentResult(Resources.GlyphiconsHalflingsRegularWoff2, "application/font-woff")},

            { "ui-bg_flat_0_aaaaaa_40x100_png", () => PngBitmapToByteStream(Resources.ui_bg_flat_0_aaaaaa_40x100_png)},
            { "ui-bg_glass_55_fbf9ee_1x400_png", () => PngBitmapToByteStream(Resources.ui_bg_glass_55_fbf9ee_1x400_png)},
            { "ui-bg_glass_65_ffffff_1x400_png", () => PngBitmapToByteStream(Resources.ui_bg_glass_65_ffffff_1x400_png)},
            { "ui-bg_glass_75_dadada_1x400_png", () => PngBitmapToByteStream(Resources.ui_bg_glass_75_dadada_1x400_png)},
            { "ui-bg_glass_75_e6e6e6_1x400_png", () => PngBitmapToByteStream(Resources.ui_bg_glass_75_e6e6e6_1x400_png)},
            { "ui-bg_glass_75_ffffff_1x400_png", () => PngBitmapToByteStream(Resources.ui_bg_glass_75_ffffff_1x400_png)},
            { "ui-bg_highlight_soft_75_cccccc_1x100_png", () => PngBitmapToByteStream(Resources.ui_bg_highlight_soft_75_cccccc_1x100_png)},
            { "ui-bg_inset_soft_95_fef1ec_1x100_png", () => PngBitmapToByteStream(Resources.ui_bg_inset_soft_95_fef1ec_1x100_png)},
            { "ui-icons_222222_256x240_png", () => PngBitmapToByteStream(Resources.ui_icons_222222_256x240_png)},
            { "ui-icons_2e83ff_256x240_png", () => PngBitmapToByteStream(Resources.ui_icons_2e83ff_256x240_png)},
            { "ui-icons_454545_256x240_png", () => PngBitmapToByteStream(Resources.ui_icons_454545_256x240_png)},
            { "ui-icons_888888_256x240_png", () => PngBitmapToByteStream(Resources.ui_icons_888888_256x240_png)},
            { "ui-icons_cd0a0a_256x240_png", () => PngBitmapToByteStream(Resources.ui_icons_cd0a0a_256x240_png)},
            { "ui-icons_f6cf3b_256x240_png", () => PngBitmapToByteStream(Resources.ui_icons_f6cf3b_256x240_png)},
        };

        private static readonly Dictionary<string, Func<ContentResult>> StringResources = new Dictionary<string, Func<ContentResult>> 
        {
            { "GlyphiconsHalflingsReguarSvg", () => new ContentResult { Content = Resources.GlyphiconsHalflingsReguarSvg, ContentType = "image/svg+xml" }},
            { "bootstrap_css_map", () => new ContentResult { Content = Resources.bootstrap_css_map, ContentType = "application/text" }},
        };


        public static string MakeAction(UrlHelper urlHelper, string resourceName)
        {
            if (StringResources.ContainsKey(resourceName))
            {
				return urlHelper.Action("GetStringResource", "Rabbit", new { name = resourceName });
            }
            
            if (BinaryResources.ContainsKey(resourceName))
            {
				return urlHelper.Action("GetBinaryResource", "Rabbit", new { name = resourceName });
            }

            return null;           
        }

        [HttpGet]
        [OutputCache(Duration = 3600)]
        public ContentResult GetStringResource(string name)
        {
            if (StringResources.ContainsKey(name))
            {
                return StringResources[name]();
            }

            return null;
        }

        [HttpGet]
        [OutputCache(Duration = 3600)]
        public FileResult GetBinaryResource(string name)
        {
            if (BinaryResources.ContainsKey(name))
            {
                return BinaryResources[name]();
            }

            return null;
        }

        private static FileStreamResult PngBitmapToByteStream(Bitmap bitmap) 
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(ms, "image/png");
        }
    }
}