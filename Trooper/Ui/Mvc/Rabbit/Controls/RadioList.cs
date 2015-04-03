namespace Trooper.Ui.Mvc.Rabbit.Controls
{
    using System.Collections.Generic;
    using System.Linq;

    using Trooper.Ui.Mvc.Rabbit.Controls.Options;

    public class RadioList<TOptionKey, TOptionValue> : FormControl
    {
		public List<Option<TOptionKey, TOptionValue>> Options { get; set; }

        public TOptionKey SelectedOption { get; set; }

        public bool Inline { get; set; }

        public int? ScrollHeight { get; set; }

        public InputTextSizes? TextSize { get; set; }
    }
}
