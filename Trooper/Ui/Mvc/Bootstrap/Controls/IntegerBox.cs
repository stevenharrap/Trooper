using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    public class IntegerBox : TextBox
    {
        public new int? Value { get; set; }

        public int? Minimum { get; set; }

        public int? Maximum { get; set; }
    }
}
