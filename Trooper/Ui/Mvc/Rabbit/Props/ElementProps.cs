using System.Collections.Generic;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Thorny;
using Trooper.Thorny.Business.Response;
using Trooper.Thorny.Utility;
using Trooper.Ui.Interface.Mvc.Rabbit.Props;

namespace Trooper.Ui.Mvc.Rabbit.Props
{
	public class ElementProps : IElementProps
    {
        public ElementProps()
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

        public IList<Message> Messages { get; set; }
    }
}
