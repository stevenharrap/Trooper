using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
	public class Popover : HtmlControl
	{
		public string Selector { get; set; }

		public Func<object, IHtmlString> Content { get; set; }

		public PopoverPlacements Placement { get; set; }
	}
}
