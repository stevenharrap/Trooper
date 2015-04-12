using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trooper.Testing.Web.Models;
using Trooper.Ui.Mvc.Rabbit.Models;

namespace Trooper.Testing.Web.Models
{
    public class TestTableModel
    {
        public IEnumerable<BaseballMaster> BaseballMasters { get; set; }

        public TableModel TbMdl001 { get; set; }
    }
}