namespace Trooper.Testing.CoreShop.Business.Support
{
    using Autofac;
    using Trooper.BusinessOperation2.Injection;
    using Trooper.Testing.CoreShop.Business.Operation;
    using Trooper.Testing.CoreShop.Facade;
    using Trooper.Testing.CoreShop.Interface.Business.Operation;
    using Trooper.Testing.CoreShop.Interface.Business.Support;
    using Trooper.Testing.CoreShop.Interface.Model;
    using Trooper.Testing.CoreShop.Model;

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
