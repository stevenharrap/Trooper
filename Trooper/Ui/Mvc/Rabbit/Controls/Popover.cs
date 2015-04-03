using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Trooper.Ui.Mvc.Rabbit.Controls
{
	public class Popover : HtmlControl
	{
        public Popover()
        {
            this.PlacementAutoAssist = true;
            this.Placement = PopoverPlacements.Bottom;
			this.Behaviour = PopoverBehaviour.Hover;
        }

		public string Selector { get; set; }

		public Func<object, IHtmlString> ContentFunc { 
            set 
            {
                this.Content = value.Invoke(null).ToString();
            }
        }

        public string Content
        {
            get;
            set;
        }

		public PopoverPlacements Placement { get; set; }

		public PopoverBehaviour Behaviour { get; set; }

        public bool PlacementAutoAssist { get; set; }
	}
}
