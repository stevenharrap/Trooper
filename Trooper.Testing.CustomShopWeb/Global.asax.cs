using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Trooper.Testing.CustomShopApi;
using Trooper.Thorny.Configuration;

namespace Trooper.Testing.CustomShopWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = BusinessModuleBuilder.StartBusinessApp<ShopAppModule>();

            Application.Add("businessAppContainer", container);            
        }
    }
}
