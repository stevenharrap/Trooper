using Trooper.Ui.Mvc.Rabbit.Controls;

namespace Trooper.Ui.Mvc.Rabbit.Props
{
    public class IntegerBoxProps : TextBoxProps
    {
        public new int? Value { get; set; }

        public int? Minimum { get; set; }

        public int? Maximum { get; set; }
    }
}
