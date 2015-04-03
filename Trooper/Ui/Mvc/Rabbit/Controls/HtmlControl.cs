namespace Trooper.Ui.Mvc.Rabbit.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.BusinessOperation2;
    using Trooper.BusinessOperation2.Interface.OperationResponse;
    using Trooper.BusinessOperation2.Utility;

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

        public List<IMessage> Messages { get; set; }
    }
}
