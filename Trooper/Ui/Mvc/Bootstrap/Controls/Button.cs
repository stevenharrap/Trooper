namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    using System.Collections.Generic;

    public class Button : FormControl
    {
        public Button()
        {
            this.ButtonType = ButtonTypes.Default;
            this.LoadingScreenTitle = "Working...";
        }

        public string Value { get; set; }

        public ButtonTypes ButtonType { get; set; }

        public bool Submit { get; set; }

        public string Url { get; set; }

        public bool TargetNewWindow { get; set; }

        public string ToolTip { get; set; }

        public string Icon { get; set; }

        public Dictionary<string, string> Attrs { get; set; }

        public List<string> ButtonClasses { get; set; }

        public bool LaunchLoadingOnclick { get; set; }

        public string LoadingScreenTitle { get; set; }

        public bool ConfirmOnClick { get; set; }

        public string ConfirmTitle { get; set; }
    }
}
