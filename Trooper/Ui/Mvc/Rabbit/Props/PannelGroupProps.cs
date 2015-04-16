using System.Collections.Generic;

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
