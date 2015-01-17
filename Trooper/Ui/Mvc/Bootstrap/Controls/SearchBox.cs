using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    public class SearchBox : FormControl
    {
        public SearchBox()
        {
            this.ScrollHeight = 150;
            this.PopoverPlacement = PopoverPlacements.Bottom;
        }

        public string DataSourceUrl { get; set; }

        public string SearchValueField { get; set; }

        public string SearchTextField { get; set; }

        public string SelectedTextField { get; set; }

        public string SelectEvent { get; set; }

        public string Value { get; set; }

        public string Text { get; set; }

        public int ScrollHeight { get; set; }

        public int? PopoutWidth { get; set; }

        public InputTextSizes? TextSize { get; set; }

        public PopoverPlacements PopoverPlacement { get; set; }
    }
}
