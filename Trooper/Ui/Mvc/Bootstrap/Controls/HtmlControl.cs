namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    using System.Collections.Generic;
    using System.Linq;

    using Trooper.BusinessOperation.Business;
    using Trooper.BusinessOperation.Utility;

    public class HtmlControl
    {
        public HtmlControl()
        {
            this.Visible = true;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public bool? ShowTitle { get; set; }

        public bool Visible { get; set; }

        public bool? Enabled { get; set; }

        public bool WarnOnLeave { get; set; }

        public MessageAlertLevel? WorstMessageLevel
        {
            get
            {
                return MessageUtility.GetWorstMessageLevel(this.Messages);
            }
        }

        public List<Message> Messages { get; set; }
    }
}
