﻿namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    using System.Collections.Generic;

    using Trooper.Ui.Mvc.Bootstrap.Controls.Options;

    public class CheckBoxList<TOptionKey, TOptionValue> : FormControl
    {
        public OptionList<TOptionKey, TOptionValue> Options { get; set; }

        public List<TOptionKey> SelectedOptions { get; set; }

        public bool Inline { get; set; }

        public int? ScrollHeight { get; set; }
    }
}
