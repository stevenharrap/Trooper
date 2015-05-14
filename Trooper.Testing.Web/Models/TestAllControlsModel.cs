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

	public enum Vegetables
	{
		Carrot,
		Potato,
		Pumpkin,
		Cabage
	}

	public class TestAllControlsModel
    {
        public int? TestNum01 { get; set; }

        public int TestNum02 { get; set; }

        public decimal? TestDec01 { get; set; }

        public decimal? TestDec02 { get; set; }

        public decimal TestDec03 { get; set; }

        public decimal? TestDec04 { get; set; }

        public string TestTx01 { get; set; }

        public string TestTx02 { get; set; }

        public string TestTx03 { get; set; }

        public bool TestBl01 { get; set; }

        public string TestSng01 { get; set; }

        public string TestSng02 { get; set; }

		public Vegetables TestSng03 { get; set; }

        public List<string> TestMlt01 { get; set; }

        public List<string> TestMlt02 { get; set; }

        public DateTime TestDt01 { get; set; }

        public DateTime TestDt02 { get; set; }

        public DateTime? TestDt03 { get; set; }

        public DateTime? TestDt04 { get; set; }

        public DateTime? TestDt05 { get; set; }

        public UploadModel TestFl01 { get; set; }

        public UploadModel TestFl02 { get; set; }
        
        public string ShowTitles { get; set; }

        public bool AllEnabled { get; set; }

        public MessageAlertLevel? MessageAlertLevel { get; set; }

        [ReadOnly(true)]
        public IResponse OperationResponse { get; set; }

        [ReadOnly(true)]
        public List<Option<string, string>> Fruits
        {
            get
            {
                return new List<Option<string, string>>
                {
                    new Option<string, string>("Org", "Orange" ),
                    new Option<string, string>( "Apl", "Apple" ),
                    new Option<string, string>( "Str", "Strawberry" ),
                    new Option<string, string>( "Pre", "Pare" ),
                    new Option<string, string>( "Grp", "Grape" ),
                    new Option<string, string>( "Bna", "Banana" ),
                    new Option<string, string>( "Mng", "Mango" ),
                    new Option<string, string>( "Tmt", "Tomatoe" ),
                };
            }
        }

        [ReadOnly(true)]
        public List<Option<string, string>> TitleOptions
        {
            get
            {
                return new List<Option<string, string>> 
                { 
                    new Option<string, string>( string.Empty, "Default" ), 
                    new Option<string, string>( "Show", "Show" ), 
                    new Option<string, string> ( "Hide", "Hide" ) 
                };
            }
        }
    }
}