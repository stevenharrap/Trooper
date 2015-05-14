﻿using System.Collections.Generic;
using Trooper.Thorny;
using Trooper.Thorny.Interface.OperationResponse;

namespace Trooper.Ui.Interface.Mvc.Rabbit.Props
{
	public interface IElementProps
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