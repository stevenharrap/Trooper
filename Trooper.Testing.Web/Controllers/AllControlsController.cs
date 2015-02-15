//--------------------------------------------------------------------------------------
// <copyright file="BsfTestController.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Testing.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Shop.DemoWeb.Models;

    using Trooper.Ui.Mvc.Bootstrap.Models;
    using Trooper.BusinessOperation2.OperationResponse;
    using Trooper.BusinessOperation2;
    using Trooper.BusinessOperation2.Utility;
    using Trooper.BusinessOperation2.Interface.OperationResponse;

    /// <summary>
    /// Controller for BSF test page
    /// </summary>
    public class AllControlsController : Controller
    {
        /// <summary>
        /// The view file.
        /// </summary>
        private const string ViewFile = "~/Views/AllControlsView.cshtml";

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new AllControlsModel
                            {
                                TestFl01 = new UploadModel(),
                                TestFl02 = new UploadModel()
                            };

            Persist(model);

            return this.View(ViewFile, model);
        }

        [HttpPost]
        public ActionResult Submit(AllControlsModel model)
        {
            Persist(model);            

            return this.View(ViewFile, model);
        }
        
        [HttpPost]
        public ActionResult Add(AllControlsModel model)
        {
            Persist(model);

            MessageUtility.Messages(MessageAlertLevel.Success).Add("Adding has occured", model.OperationResponse);

            return this.View(ViewFile, model);
        }

        [HttpPost]
        public ActionResult Update(AllControlsModel model)
        {
            Persist(model);

            MessageUtility.Messages(MessageAlertLevel.Success).Add("Updating has occured", model.OperationResponse);

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
        private static void Persist(AllControlsModel currentModel)
        {
            //UploadHelper.

            if (currentModel.MessageAlertLevel != null)
            {
                currentModel.OperationResponse = currentModel.OperationResponse ?? new Response();
                
                var level = (MessageAlertLevel)currentModel.MessageAlertLevel;

                currentModel.OperationResponse.Messages = new List<IMessage> 
                {
                    MessageUtility.Messages(level).Make("Test Num01 message test", () => currentModel.TestNum01),
                    MessageUtility.Messages(level).Make("Test Num02 message test", () => currentModel.TestNum02),
                    MessageUtility.Messages(level).Make("Test Dec01 message test", () => currentModel.TestDec01),
                    MessageUtility.Messages(level).Make("Test Dec02 message test", () => currentModel.TestDec02),
                    MessageUtility.Messages(level).Make("Test Dec03 message test", () => currentModel.TestDec03),
                    MessageUtility.Messages(level).Make("Test Dec04 message test", () => currentModel.TestDec04),
                    MessageUtility.Messages(level).Make("Test Tx01 message test", () => currentModel.TestTx01),
                    MessageUtility.Messages(level).Make("Test Tx02 message test", () => currentModel.TestTx02),
                    MessageUtility.Messages(level).Make("Test Tx03 message test", () => currentModel.TestTx03),
                    MessageUtility.Messages(level).Make("Test Bl01 message test", () => currentModel.TestBl01),
                    MessageUtility.Messages(level).Make("Test Sng01 message test", () => currentModel.TestSng01),
                    MessageUtility.Messages(level).Make("Test Sng02 message test", () => currentModel.TestSng02),
                    MessageUtility.Messages(level).Make("Test Mlt01 message test", () => currentModel.TestMlt01),
                    MessageUtility.Messages(level).Make("Test Mlt02 message test", () => currentModel.TestMlt02),
                    MessageUtility.Messages(level).Make("Test Dt01 message test", () => currentModel.TestDt01),
                    MessageUtility.Messages(level).Make("Test Dt02 message test", () => currentModel.TestDt02),
                    MessageUtility.Messages(level).Make("Test Dt03 message test", () => currentModel.TestDt03),
                    MessageUtility.Messages(level).Make("Test Dt04 message test", () => currentModel.TestDt04),
                    MessageUtility.Messages(level).Make("Test Dt05 message test", () => currentModel.TestDt05),
                    MessageUtility.Messages(level).Make("Test Fl01 message test", () => currentModel.TestFl01),
                    MessageUtility.Messages(level).Make("Test Fl02 message test", () => currentModel.TestFl02)
                };
            }
        }
    }
}