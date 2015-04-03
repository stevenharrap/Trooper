namespace Trooper.Ui.Mvc.Rabbit.Controls
{
    using System.Collections.Generic;

    public class TextareaBox : FormControl
    {
        public string Value { get; set; }        

        public int? MaxLength { get; set; }

        public int? Rows { get; set; }        
    }
}