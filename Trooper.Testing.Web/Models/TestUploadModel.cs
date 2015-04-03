//--------------------------------------------------------------------------------------
// <copyright file="BsfTestModel.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Trooper.BusinessOperation2;
using Trooper.BusinessOperation2.Interface.OperationResponse;
using Trooper.Ui.Mvc.Rabbit.Controls.Options;
using Trooper.Ui.Mvc.Rabbit.Models;

namespace Trooper.Testing.Web.Models
{
	public class TestUploadModel
    {
        public UploadModel TestFl01 { get; set; }

        public UploadModel TestFl02 { get; set; }

        public UploadModel TestFl03 { get; set; }
        
        public UploadModel TestFl04 { get; set; }
        
        public UploadModel TestFl05 { get; set; }

    }
}