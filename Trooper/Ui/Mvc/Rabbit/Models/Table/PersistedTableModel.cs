using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Ui.Mvc.Rabbit.Models.Table;
using Trooper.Ui.Mvc.Rabbit.Props.Table;

namespace Trooper.Ui.Mvc.Rabbit.Models.Table
{
    public class PersistedTableModel
    {
        public IList<string> Selected { get; set; }

        public int PageNumber { get; set; }

        public IDictionary<string, SortInfo> Sorting { get; set; }
    }    
}
