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
    using Trooper.Ui.Mvc.Rabbit.Models;
    using Trooper.Thorny.OperationResponse;
    using Trooper.Thorny;
    using Trooper.Thorny.Utility;
    using Trooper.Thorny.Interface.OperationResponse;
    using System;

    /// <summary>
    /// Controller for BSF test page
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The view file.
        /// </summary>
        private const string ViewFile = "~/Views/HomeView.cshtml";

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new HomeModel();

            Persist(model);

            return this.View(ViewFile, model);
        }

        [HttpPost]
        public ActionResult Submit(HomeModel model)
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
        private static void Persist(HomeModel currentModel)
        {
        }
    }
}