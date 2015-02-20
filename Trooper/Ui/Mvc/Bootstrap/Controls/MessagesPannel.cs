﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.BusinessOperation2;
using Trooper.BusinessOperation2.Interface.OperationResponse;
using Trooper.BusinessOperation2.Utility;

namespace Trooper.Ui.Mvc.Bootstrap.Controls
{
    public class MessagesPannel : HtmlControl
    {
	    private int columns = 1;

	    public int Columns
	    {
		    get { return this.columns; }
			set 
			{
				if (value == 1 || value == 2 || value == 3 || value == 4 || value == 6)
				{
					this.columns = value;
				}
			}
	    }
    }
}
