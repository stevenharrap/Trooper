using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Trooper.Ui.Mvc.Rabbit.Props
{
	public class ModalWindowProps : ElementProps
    {
        public ModalWindowProps()
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
