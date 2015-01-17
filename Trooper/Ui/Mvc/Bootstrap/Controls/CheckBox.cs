using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    public class CheckBox : FormControl
    {
        public string Value { get; set; }

        public bool? Checked { get; set; }

        public bool Inline { get; set; }
    }
}
