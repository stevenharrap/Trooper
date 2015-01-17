using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    public class PannelGroup : HtmlControl
    {
        public PannelGroup()
        {
            this.ErrorMode = PanelGroupErrorModes.ScrollToTop;
        }

        public List<PanelGroupItem> Items { get; set; }
        
        public PanelGroupErrorModes ErrorMode { get; set; }

    }
}
