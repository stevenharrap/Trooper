namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.WebPages;

    public class ModalWindow : HtmlControl
    {
        public ModalWindow()
        {
            this.IncCloseButton = true;

            this.FrameHeight = 400;
        }

        public List<MvcHtmlString> Buttons { get; set; }

        public bool IncCloseButton { get; set; }

        public Func<object, HelperResult> Content { get; set; }

        public string FrameUrl { get; set; }

        public int FrameHeight { get; set; }
    }
}
