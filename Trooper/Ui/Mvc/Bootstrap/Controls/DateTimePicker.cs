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
            this.UtcOffset = 600;

            this.PopoverPlacement = PopoverPlacements.Bottom;

            this.DateTimeFormat = DateTimeFormat.DateAndTime;
        }

        public DateTime? Value { get; set; }

		public DateTime? Minimum { get; set; }

		public DateTime? Maximum { get; set; }

        public DateTimeFormat DateTimeFormat { get; set; }

        public int UtcOffset { get; set; }

        public InputTextSizes? TextSize { get; set; }

        public PopoverPlacements PopoverPlacement { get; set; }
    }
}
