namespace Trooper.Testing.CustomShopWeb.Controllers
{
    using Autofac;
    using System.ServiceModel;
    using System.Web.Mvc;
    using Trooper.Testing.CustomShopWeb.Models;
    using Trooper.Thorny.Business.Security;

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

            ChannelFactory<IOutletBo> scf = new ChannelFactory<IOutletBo>(
                new NetHttpBinding(),
                "http://localhost:8000/Trooper.Testing.CustomShopApi.Interface.Business.Operation.IShopBo");

            var shopBo = scf.CreateChannel();

            var allShops = shopBo.GetAll(new Identity { Username = "ValidTestUser" });


            return this.View(@"~\Views\Home.cshtml", model);
        }
    }
}