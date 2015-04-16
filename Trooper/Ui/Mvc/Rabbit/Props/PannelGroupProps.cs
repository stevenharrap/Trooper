using System.Collections.Generic;
using Trooper.Ui.Mvc.Rabbit.Props.PanelGroup;

namespace Trooper.Ui.Mvc.Rabbit.Props
{
    public class PannelGroupProps : ElementProps
    {
        public PannelGroupProps()
        {
            this.ErrorMode = PanelGroupErrorModes.ScrollToTop;
        }

        public List<PanelGroupItem> Items { get; set; }
        
        public PanelGroupErrorModes ErrorMode { get; set; }

    }
}
