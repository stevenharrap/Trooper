using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trooper.Testing.Web.Models;

namespace Trooper.Testing.Web.Models
{
    public class TestTableModel
    {
        public IEnumerable<BaseballMaster> BaseballMasters { get; set; }
    }
}