using Trooper.Ui.Mvc.Rabbit.Controls;

namespace Trooper.Ui.Mvc.Rabbit.Props
{
    public class CheckBoxProps : InputProps
    {
        public string Value { get; set; }

        public bool? Checked { get; set; }

        public bool Inline { get; set; }
    }
}
