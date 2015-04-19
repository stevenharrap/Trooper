using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Ui.Mvc.Rabbit.Models.Table
{
    public class PersistedSortInfo
    {
        public SortDirection? Direction { get; set; }

        public int Importance { get; set; }
    }
}
