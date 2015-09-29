namespace Trooper.Testing.CustomShopApi.Business.Support.OutletSupport
{
    using Autofac;
    using System.Collections.Generic;
    using Trooper.DynamicServiceHost;
    using Trooper.Interface.DynamicServiceHost;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Configuration;
    using Trooper.Testing.CustomShopApi.Business.Model;
    using Trooper.Testing.CustomShopApi.Business.Operation;
    using Trooper.Testing.CustomShopApi.Facade.ShopSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.OutletSupport;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Thorny.Business.Response;
    using Trooper.Thorny.Configuration;
    using Trooper.Utility;
    using System.Linq;

    public class OutletConfiguration
    {
        public static void AddShop(ContainerBuilder builder)
        {
            var component = new BusinessComponent<OutletEnt, Outlet>(builder);
            component.RegisterFacade<OutletFacade, IOutletFacade>();
            component.RegisterAuthorization<OutletAuthorization, IOutletAuthorization>();
            component.RegisterValidation<OutletValidation, IOutletValidation>();
            component.RegisterBusinessCore<OutletBusinessCore, IOutletBusinessCore>();
            component.RegisterBusinessOperation<OutletBo, IOutletBo>();            

            component.RegisterDynamicServiceHost(new BusinessHostInfo
            {
                BaseAddress = "http://localhost:8000",
                Mappings = new List<ClassMapping>
                    {
                        ClassMapping.Make<ISingleResponse<ProductInOutlet>, SingleResponse<ProductInOutlet>>(),
                        ClassMapping.Make<ISaveResponse<ProductInOutlet>, SaveResponse<ProductInOutlet>>()
                    },
            });

            BusinessModule.AddComponent(component);
        }
    }
}
