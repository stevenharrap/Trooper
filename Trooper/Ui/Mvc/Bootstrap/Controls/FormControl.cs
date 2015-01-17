namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    using System.Collections.Generic;

    public class FormControl : HtmlControl
    {
        public string Name { get; set; }

        public List<string> FormGroupClasses { get; set; }
    }
}
