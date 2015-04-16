using System.Collections.Generic;
using Trooper.Ui.Mvc.Rabbit.Controls.Options;

namespace Trooper.Ui.Mvc.Rabbit.Props
{
	public class RadioListProps<TOptionKey, TOptionValue> : InputProps
    {
		public List<Option<TOptionKey, TOptionValue>> Options { get; set; }

        public TOptionKey SelectedOption { get; set; }

        public bool Inline { get; set; }

        public int? ScrollHeight { get; set; }

        public InputTextSizes? TextSize { get; set; }
    }
}
