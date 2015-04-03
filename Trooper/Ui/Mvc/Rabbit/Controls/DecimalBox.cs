using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Ui.Mvc.Rabbit.Controls
{
    public class DecimalBox : TextBox
    {
        public new decimal? Value { get; set; }

        public decimal? Minimum { get; set; }

        public decimal? Maximum { get; set; }

        public int? DecimalDigits { get; set; }
    }
}
