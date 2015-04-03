namespace Trooper.Ui.Mvc.Rabbit.Controls
{
    using System.Collections.Generic;

    using Trooper.Ui.Mvc.Rabbit.Controls.Options;

    public class CheckBoxList<TOptionKey, TOptionValue> : FormControl
    {
		public List<Option<TOptionKey, TOptionValue>> Options { get; set; }

        public List<TOptionKey> SelectedOptions { get; set; }

        public bool Inline { get; set; }

        public int? ScrollHeight { get; set; }
    }
}
