using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
using Trooper.Testing.CustomShopWeb.Models;
using Trooper.Thorny.Business.Security;
using Trooper.Thorny.Configuration;

namespace Trooper.Testing.CustomShopWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //var x = StringComparison.OrdinalIgnoreCase;
            var container = HttpContext.Application["businessAppContainer"] as IContainer;

            var model = new HomeModel();

            if (container != null)
            {
                //model.AllServices = BusinessModuleBuilder.GetAllServices(container);
            };

            ChannelFactory<IShopBo> scf = new ChannelFactory<IShopBo>(
                new NetHttpBinding(),
                "http://localhost:8000/Trooper.Testing.CustomShopApi.Interface.Business.Operation.IShopBo");

            var shopBo = scf.CreateChannel();

            var allShops = shopBo.GetAll(new Identity { Username = "ValidTestUser" });


            return this.View(@"~\Views\Home.cshtml", model);
        }
    }
}