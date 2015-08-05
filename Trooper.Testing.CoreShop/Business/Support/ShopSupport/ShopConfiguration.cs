namespace Trooper.Testing.CustomShopApi.Business.Support.ShopSupport
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
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Thorny.Business.Response;
    using Trooper.Thorny.Configuration;

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

            component.RegisterServiceHost(
                new BusinessHostInfo 
                {
                    BaseAddress = "http://localhost:8000",
                    Mappings = new List<ClassMapping> 
                    {
                        ClassMapping.Make<ISingleResponse<ProductInShop>, SingleResponse<ProductInShop>>(),
                        ClassMapping.Make<ISaveResponse<ProductInShop>, SaveResponse<ProductInShop>>()
                    },
                    SearchMappings = new List<ClassMapping>()
                    {
                        BusinessHostInfo.NewSearch<ShopAddressSearch>(),
                        BusinessHostInfo.NewSearch<ShopNameSearch>()
                    }                    
                });

            BusinessModule.AddComponent(component);
        }
    }
}
