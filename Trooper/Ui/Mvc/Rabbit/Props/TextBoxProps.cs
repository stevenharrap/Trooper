namespace Trooper.Ui.Mvc.Rabbit.Props
{
	public class TextBoxProps : InputProps
    {
        public string Value { get; set; }               

        public int? MaxLength { get; set; }
        
        public InputTextSizes? TextSize { get; set; }
    }
}