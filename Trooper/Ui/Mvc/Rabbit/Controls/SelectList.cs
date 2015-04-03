using Trooper.Utility;

namespace Trooper.Ui.Mvc.Rabbit.Controls
{
    using System.Collections.Generic;
    using System.Linq;

    using Trooper.Ui.Mvc.Rabbit.Controls.Options;

	public class SelectList<TOption> : SelectList<TOption, TOption>
	{
	}

    public class SelectList<TOptionKey, TOptionValue> : FormControl
    {
	    public SelectList()
	    {
			this.Options = new List<Option<TOptionKey, TOptionValue>>();
	    }

		public List<Option<TOptionKey, TOptionValue>> Options { get; set; }

		public void Add(TOptionKey key, TOptionValue value)
		{
			this.Options.Add(new Option<TOptionKey, TOptionValue>(key, value));
		}

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
