using System.Collections.Generic;
using Trooper.Ui.Mvc.Rabbit.Controls;
using Trooper.Ui.Mvc.Rabbit.Controls.Options;

namespace Trooper.Ui.Mvc.Rabbit.Props
{
	public class CheckBoxListProps<TOptionKey, TOptionValue> : InputProps
    {
		public IList<Option<TOptionKey, TOptionValue>> Options { get; set; }

        public IList<TOptionKey> SelectedOptions { get; set; }

        public bool Inline { get; set; }

        public int? ScrollHeight { get; set; }
    }
}
