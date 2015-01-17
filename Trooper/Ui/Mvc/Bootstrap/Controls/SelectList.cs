namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    using System.Collections.Generic;
    using System.Linq;

    using Trooper.Ui.Mvc.Bootstrap.Controls.Options;

    public class SelectList<TOptionKey, TOptionValue> : FormControl
    {
        public OptionList<TOptionKey, TOptionValue> Options { get; set; }

        public List<TOptionKey> SelectedOptions { get; set; }

        public TOptionKey SelectedOption
        {
            get
            {
                return this.SelectedOptions != null && this.SelectedOptions.Any()
                           ? this.SelectedOptions.First()
                           : default(TOptionKey);
            }

            set
            {
                if (this.SelectedOptions == null)
                {
                    this.SelectedOptions = new List<TOptionKey>();
                }

                if (this.SelectedOptions.Any())
                {
                    this.SelectedOptions[0] = value;
                }
                else
                {
                    this.SelectedOptions.Add(value);
                }
            }
        }

        public bool AllowMultiple { get; set; }

        public bool Inline { get; set; }

        public int? ScrollHeight { get; set; }

        public InputTextSizes? TextSize { get; set; }

        public bool IncludeBlank { get; set; }

        public string BlankText { get; set; }
    }
}
