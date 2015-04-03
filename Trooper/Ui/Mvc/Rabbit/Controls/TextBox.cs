namespace Trooper.Ui.Mvc.Rabbit.Controls
{
    using System.Collections.Generic;

    public class TextBox : FormControl
    {
        public string Value { get; set; }               

        public int? MaxLength { get; set; }
        
        public InputTextSizes? TextSize { get; set; }
    }
}