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
    using Trooper.Thorny;
    using Trooper.Thorny.Utility;
    using Trooper.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Response;

    /// <summary>
    /// Controller for BSF test page
    /// </summary>
    public class TestAllControlsController : Controller
    {
        /// <summary>
        /// The view file.
        /// </summary>
        private const string ViewFile = "~/Views/TestAllControlsView.cshtml";

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new TestAllControlsModel
                            {
                                TestFl01 = new UploadModel(),
                                TestFl02 = new UploadModel()
                            };

            Persist(model);

            return this.View(ViewFile, model);
        }

        [HttpPost]
        public ActionResult Submit(TestAllControlsModel model)
        {
            Persist(model);            

            return this.View(ViewFile, model);
        }
        
        [HttpPost]
        public ActionResult Add(TestAllControlsModel model)
        {
            Persist(model);

            MessageUtility.Messages(MessageAlertLevel.Success).Add("Adding has occured", null, model.OperationResponse);

            return this.View(ViewFile, model);
        }

        [HttpPost]
        public ActionResult Update(TestAllControlsModel model)
        {
            Persist(model);

            MessageUtility.Messages(MessageAlertLevel.Success).Add("Updating has occured", null, model.OperationResponse);

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
        private static void Persist(TestAllControlsModel currentModel)
        {
            //UploadHelper.

            if (currentModel.MessageAlertLevel != null)
            {
                currentModel.OperationResponse = currentModel.OperationResponse ?? new Response();
                
                var level = (MessageAlertLevel)currentModel.MessageAlertLevel;

                currentModel.OperationResponse.Messages = new List<IMessage> 
                {
                    MessageUtility.Messages(level).Make("Test Num01 message test: ", null, () => currentModel.TestNum01),
                    MessageUtility.Messages(level).Make("Test Num02 message test: ", null, () => currentModel.TestNum02),
                    MessageUtility.Messages(level).Make("Test Dec01 message test: ", null, () => currentModel.TestDec01),
                    MessageUtility.Messages(level).Make("Test Dec02 message test: ", null, () => currentModel.TestDec02),
                    MessageUtility.Messages(level).Make("Test Dec03 message test: ", null, () => currentModel.TestDec03),
                    MessageUtility.Messages(level).Make("Test Dec04 message test: ", null, () => currentModel.TestDec04),
                    MessageUtility.Messages(level).Make("Test Tx01 message test: ", null, () => currentModel.TestTx01),
                    MessageUtility.Messages(level).Make("Test Tx02 message test: ", null, () => currentModel.TestTx02),
                    MessageUtility.Messages(level).Make("Test Tx03 message test: ", null, () => currentModel.TestTx03),
                    MessageUtility.Messages(level).Make("Test Bl01 message test: ", null, () => currentModel.TestBl01),
                    MessageUtility.Messages(level).Make("Test Sng01 message test: ", null, () => currentModel.TestSng01),
                    MessageUtility.Messages(level).Make("Test Sng02 message test: ", null, () => currentModel.TestSng02),
					MessageUtility.Messages(level).Make("Test Sng03 message test: ", null, () => currentModel.TestSng03),
                    MessageUtility.Messages(level).Make("Test Mlt01 message test: ", null, () => currentModel.TestMlt01),
                    MessageUtility.Messages(level).Make("Test Mlt02 message test: ", null, () => currentModel.TestMlt02),
                    MessageUtility.Messages(level).Make("Test Dt01 message test: ", null, () => currentModel.TestDt01),
                    MessageUtility.Messages(level).Make("Test Dt02 message test: ", null, () => currentModel.TestDt02),
                    MessageUtility.Messages(level).Make("Test Dt03 message test: ", null, () => currentModel.TestDt03),
                    MessageUtility.Messages(level).Make("Test Dt04 message test: ", null, () => currentModel.TestDt04),
                    MessageUtility.Messages(level).Make("Test Dt05 message test: ", null, () => currentModel.TestDt05),
                    MessageUtility.Messages(level).Make("Test Fl01 message test: ", null, () => currentModel.TestFl01),
                    MessageUtility.Messages(level).Make("Test Fl02 message test: ", null, () => currentModel.TestFl02)
                };

                for (var m = 0; m < currentModel.OperationResponse.Messages.Count; m++)
                {
                    currentModel.OperationResponse.Messages[m].Content
                        += string.Format("{0}/{1}", m+1, currentModel.OperationResponse.Messages.Count);
                }
            }
        }
    }
}