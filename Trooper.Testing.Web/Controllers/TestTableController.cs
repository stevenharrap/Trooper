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
    using Trooper.BusinessOperation2.OperationResponse;
    using Trooper.BusinessOperation2;
    using Trooper.BusinessOperation2.Utility;
    using Trooper.BusinessOperation2.Interface.OperationResponse;
    using System;
    using System.IO;
    using CsvHelper;
    using CsvHelper.Configuration;

    /// <summary>
    /// Controller for BSF test page
    /// </summary>
    public class TestTableController : Controller
    {
        /// <summary>
        /// The view file.
        /// </summary>
        private const string ViewFile = "~/Views/TestTableView.cshtml";

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new TestTableModel();
            
            Persist(model);

            return this.View(ViewFile, model);
        }

        [HttpPost]
        public ActionResult Submit(TestTableModel model)
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
        private void Persist(TestTableModel currentModel)
        {
            var reader = new StreamReader(Server.MapPath(@"~\Content\BaseballTestData\Master.csv"));
            var csv = new CsvReader(reader);
            csv.Configuration.IsHeaderCaseSensitive = false;
            currentModel.BaseballMasters = csv.GetRecords<BaseballMaster>();
            //currentModel.BaseballMasters

        }
    }
}