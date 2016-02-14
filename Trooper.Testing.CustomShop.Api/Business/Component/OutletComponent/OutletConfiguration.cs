namespace Trooper.Testing.CustomShop.Api.Business.Support.OutletComponent
{
    using Autofac;
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Response;
    using ShopModel.Model;
    using ShopPoco;
    using Thorny.Business.Response;
    using Thorny.Configuration;
    using Utility;
    using Facade.ShopSupport;
    using Interface.Business.Support.OutletComponent;
    using Operation;
    using Interface.Business.Operation;
    using Model;

    public class OutletConfiguration
    {
        public static void AddShop(ContainerBuilder builder)
        {
            var component = new BusinessComponent<OutletEnt, Outlet>(builder);
            component.RegisterFacade<OutletFacade, IOutletFacade>();
            component.RegisterAuthorization<OutletAuthorization, IOutletAuthorization>();
            component.RegisterValidation<OutletValidation, IOutletValidation>();
            component.RegisterBusinessCore<OutletBusinessCore, IOutletBusinessCore>();
            component.RegisterCache<OutletCache, IOutletCache>();
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
