namespace Trooper.Ui.Mvc.Rabbit.Controls
{
    using System.Collections.Generic;

    public enum Method
    {
        Post,
        Get
    }

    public class FormControl : HtmlControl
    {
        public FormControl()
        {
            this.Method = Controls.Method.Post;
        }

        public string Name { get; set; }

        public List<string> FormGroupClasses { get; set; }

        public string Action { get; set; }

        public Method Method { get; set; }

        /// <summary>
        /// Gets or sets if all controls that support the disabled or readonly parameter 
        /// enabled or disabled (readonly in some cases) by default.
        /// Giving a control a specific disabled or readonly value will over-ride this.
        /// By default this is null and so are the enabled/readonly properties so
        /// result state of a control will be that it is access-able by default.
        /// </summary>
        public bool? ControlsEnabled { get; set; }

        public bool? ShowTitles { get; set; }
    }
}
