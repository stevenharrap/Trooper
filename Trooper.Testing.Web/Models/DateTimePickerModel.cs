﻿//--------------------------------------------------------------------------------------
// <copyright file="BsfTestModel.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Trooper.BusinessOperation2;
using Trooper.BusinessOperation2.Interface.OperationResponse;
using Trooper.Ui.Mvc.Bootstrap.Controls.Options;
using Trooper.Ui.Mvc.Bootstrap.Models;

namespace Trooper.Testing.Web.Models
{
	public class DateTimePickerModel
    {
        public DateTime TestDt01 { get; set; }

        public DateTime TestDt02 { get; set; }

        public DateTime? TestDt03 { get; set; }

        public DateTime? TestDt04 { get; set; }

        public DateTime? TestDt05 { get; set; }
    }
}