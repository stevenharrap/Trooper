//--------------------------------------------------------------------------------------
// <copyright file="BsfTestController.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.Testing.Web.Models;

namespace Trooper.Testing.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Trooper.Ui.Mvc.Bootstrap.Models;
    using Trooper.BusinessOperation2.OperationResponse;
    using Trooper.BusinessOperation2;
    using Trooper.BusinessOperation2.Utility;
    using Trooper.BusinessOperation2.Interface.OperationResponse;
    using System;

    /// <summary>
    /// Controller for BSF test page
    /// </summary>
	public class DateTimePickerController : Controller
    {
        /// <summary>
        /// The view file.
        /// </summary>
		private const string ViewFile = "~/Views/DateTimePickerView.cshtml";

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
	        var model = new DateTimePickerModel();

            model.TestDt06 = new DateTime(1977, 1, 26);

            Persist(model);

            return this.View(ViewFile, model);
        }

        [HttpPost]
		public ActionResult Submit(DateTimePickerModel model)
        {
            Persist(model);            

            return this.View(ViewFile, model);
        }
        

        /// <summary>
        /// Persists the model or makes a new one.
        /// </summary>
        /// <param name="currentModel">
        /// The current model.
        /// </param>
        /// <returns>
        /// The <see cref="BsfTestModel"/>.
        /// </returns>
		private static void Persist(DateTimePickerModel currentModel)
        {
        }
    }
}