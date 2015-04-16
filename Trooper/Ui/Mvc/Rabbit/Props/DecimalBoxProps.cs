using Trooper.Ui.Mvc.Rabbit.Controls;

namespace Trooper.Ui.Mvc.Rabbit.Props
{
    public class DecimalBoxProps : TextBoxProps
    {
        public new decimal? Value { get; set; }

        public decimal? Minimum { get; set; }

        public decimal? Maximum { get; set; }

        public int? DecimalDigits { get; set; }
    }
}
