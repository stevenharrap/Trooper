//--------------------------------------------------------------------------------------
// <copyright file="BsfTestModel.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Trooper.Thorny;
using Trooper.Thorny.Interface.OperationResponse;
using Trooper.Ui.Mvc.Rabbit.Controls.Options;
using Trooper.Ui.Mvc.Rabbit.Models;

namespace Trooper.Testing.Web.Models
{
	public class TestDateTimePickerModel
    {
        public DateTime TestDt01 { get; set; }

        public DateTime TestDt02 { get; set; }

        public DateTime? TestDt03 { get; set; }

        public DateTime? TestDt04 { get; set; }

        public DateTime? TestDt05 { get; set; }

        public DateTime? TestDt06 { get; set; }

        public DateTime? TestDt07 { get; set; }
    }
}