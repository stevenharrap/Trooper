using System.Collections.Generic;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Thorny;
using Trooper.Thorny.Business.Response;

namespace Trooper.Ui.Interface.Mvc.Rabbit.Props
{
	public interface IElementProps
    {
        bool? Enabled { get; set; }

        string Id { get; set; }

        IList<Message> Messages { get; set; }

        bool? ShowTitle { get; set; }

        string Title { get; set; }

        bool Visible { get; set; }

        bool WarnOnLeave { get; set; }

        MessageAlertLevel? WorstMessageLevel { get; }
    }
}
