using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    public class DateTimePicker : FormControl
    {
        public DateTimePicker()
        {
            this.Timezone = "+10:00";

            this.PopoverPlacement = PopoverPlacements.Bottom;

            this.DateTimeFormat = DateTimeFormat.DateAndTime;
        }

        public DateTime? Value { get; set; }

        public DateTimeFormat DateTimeFormat { get; set; }

        public string Timezone { get; set; }

        public InputTextSizes? TextSize { get; set; }

        public PopoverPlacements PopoverPlacement { get; set; }
    }
}
