namespace Trooper.Ui.Interface.Mvc.Rabbit.Controls
{
    using System;
    using System.Collections.Generic;
    using Trooper.BusinessOperation2;
    using Trooper.BusinessOperation2.Interface.OperationResponse;


    public interface IHtmlControl
    {
        bool? Enabled { get; set; }

        string Id { get; set; }

        IList<IMessage> Messages { get; set; }

        bool? ShowTitle { get; set; }

        string Title { get; set; }

        bool Visible { get; set; }

        bool WarnOnLeave { get; set; }

        MessageAlertLevel? WorstMessageLevel { get; }
    }
}
