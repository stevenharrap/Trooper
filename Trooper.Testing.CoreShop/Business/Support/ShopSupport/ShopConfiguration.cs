namespace Trooper.Testing.CustomShopApi.Business.Support.ShopSupport
{
    using System.Linq;
    using Autofac;
    using Trooper.DynamicServiceHost;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Testing.CustomShopApi.Business.Operation;
    using Trooper.Testing.CustomShopApi.Facade.ShopSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Thorny.Business.Response;
    using Trooper.Thorny.Configuration;
    using Trooper.Interface.DynamicServiceHost;

    public class ShopConfiguration
    {
        public static void AddShop(ContainerBuilder builder)
        {
            var component = new BusinessComponent<ShopEnt, Shop>(builder);
            component.RegisterFacade<ShopFacade, IShopFacade>();
            component.RegisterAuthorization<ShopAuthorization, IShopAuthorization>();
            component.RegisterValidation<ShopValidation, IShopValidation>();
            component.RegisterBusinessCore<ShopBusinessCore, IShopBusinessCore>();
            component.RegisterBusinessOperation<ShopBo, IShopBo>();

            //builder.Register(c => new SingleResponse<Product>()).As<ISingleResponse<Product>>();

            component.RegisterServiceHost(
                "http://localhost:8000", 
                hostInfoBuilt: (IHostInfo hi) => 
                {
                    //hi.Methods.FirstOrDefault(m => m.Name == "GetAll").DebugMethod = true;
                    hi.Mappings.Add(ClassMapping.Make<ISingleResponse<Product>, SingleResponse<Product>>("MyProduct"));
                });
            

            //x.Mappings.Add(ClassMapping.Make<ISingleResponse<Product>>("MyProduct"));

            BusinessModule.AddComponent(component);


            //BusinessModuleBuilder.AddBusinessCore<
            //    ShopFacade, IShopFacade, 
            //    ShopAuthorization, IShopAuthorization, 
            //    ShopValidation, IShopValidation, 
            //    ShopBusinessCore, IShopBusinessCore, 
            //    ShopBo, IShopBo, 
            //    ShopEnt, Shop>(builder);

            //BusinessHostBuilder.AddHost<ShopEnt, Shop, ShopBo, IShopBo>(builder, "http://localhost:8000", true);

            //BusinessOperationInjection.AddBusinessOperation<IShopBusinessCore, ShopBo, IShopBo, Shop, IShop>(builder);
        }
    }
}
