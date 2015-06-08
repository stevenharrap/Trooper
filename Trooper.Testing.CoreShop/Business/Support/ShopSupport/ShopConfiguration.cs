namespace Trooper.Testing.CustomShopApi.Business.Support.ShopSupport
{
    using Autofac;
    using Trooper.Thorny.Configuration;
    using Trooper.Testing.CustomShopApi.Business.Operation;
    using Trooper.Testing.CustomShopApi.Facade;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Testing.CustomShopApi.Facade.ShopSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport;

    public class ShopConfiguration
    {
        public static void AddShop(ContainerBuilder builder)
        {
            BusinessModuleBuilder.AddBusinessCore<
                ShopFacade, IShopFacade, 
                ShopAuthorization, IShopAuthorization, 
                ShopValidation, IShopValidation, 
                ShopBusinessCore, IShopBusinessCore, 
                ShopBo, IShopBo, 
                Shop, IShop>(builder);

            BusinessModuleBuilder.AddServiceHost<ShopBo, IShopBo>(builder);

            //BusinessOperationInjection.AddBusinessOperation<IShopBusinessCore, ShopBo, IShopBo, Shop, IShop>(builder);
        }
    }
}
