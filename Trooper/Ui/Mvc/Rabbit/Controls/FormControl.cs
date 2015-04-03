namespace Trooper.Ui.Mvc.Rabbit.Controls
{
    using System.Collections.Generic;

    public class FormControl : HtmlControl
    {
        public string Name { get; set; }

        public List<string> FormGroupClasses { get; set; }
    }
}
