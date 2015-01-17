namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ButtonDropDown : HtmlControl
    {
        public ButtonDropDown()
        {
            this.Direction = ButtonDropDirection.Down;

            this.ButtonType = ButtonTypes.Default;
        }

        public List<MvcHtmlString> Buttons { get; set; }

        public ButtonDropDirection Direction { get; set; }

        public ButtonTypes ButtonType { get; set; }

        public string Icon { get; set; }
    }
}
