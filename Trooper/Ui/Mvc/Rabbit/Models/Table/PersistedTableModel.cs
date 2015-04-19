using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Ui.Mvc.Rabbit.Models.Table;

namespace Trooper.Ui.Mvc.Rabbit.Models.Table
{
    public class PersistedTableModel
    {
        public PersistedTableModel()
        {
            this.PageNumber = 1;            
        }

        [ReadOnly(true)]
        public IList<IDictionary<string, object>> Selected { get; set; }

        [ReadOnly(true)]
        public int PageNumber { get; set; }

        [ReadOnly(true)]
        public IDictionary<string, PersistedSortInfo> Sorting { get; set; }
    }    
}
