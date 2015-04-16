using System.Collections.Generic;
using System.Web.Mvc;
using Trooper.Ui.Mvc.Rabbit.Controls;

namespace Trooper.Ui.Mvc.Rabbit.Props
{
	public class ButtonDropDownProps : ElementProps
    {
        public ButtonDropDownProps()
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
