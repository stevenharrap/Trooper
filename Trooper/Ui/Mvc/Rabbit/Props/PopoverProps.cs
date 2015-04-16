using System;
using System.Web;

namespace Trooper.Ui.Mvc.Rabbit.Props
{
	public class PopoverProps : ElementProps
	{
        public PopoverProps()
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
