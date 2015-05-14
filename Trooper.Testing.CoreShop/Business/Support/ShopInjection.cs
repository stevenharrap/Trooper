namespace Trooper.Testing.CustomShopApi.Business.Support
{
    using Autofac;
    using Trooper.Thorny.Injection;
    using Trooper.Testing.CustomShopApi.Business.Operation;
    using Trooper.Testing.CustomShopApi.Facade;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;

    public class ShopInjection
    {
        public static void AddShop(ContainerBuilder builder)
        {
            BusinessOperationInjection.AddBusinessCore<
                ShopFacade, IShopFacade, 
                ShopAuthorization, IShopAuthorization, 
                ShopValidation, IShopValidation, 
                ShopBusinessCore, IShopBusinessCore, 
                ShopBo, IShopBo, 
                Shop, IShop>(builder);

            //BusinessOperationInjection.AddBusinessOperation<IShopBusinessCore, ShopBo, IShopBo, Shop, IShop>(builder);
        }
    }
}
