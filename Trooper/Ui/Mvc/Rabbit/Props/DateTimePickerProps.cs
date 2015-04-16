using System;
using Trooper.Ui.Mvc.Rabbit.Controls;

namespace Trooper.Ui.Mvc.Rabbit.Props
{
    public class DateTimePickerProps : InputProps
    {
        public DateTimePickerProps()
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
