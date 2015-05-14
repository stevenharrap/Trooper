namespace Trooper.Testing.DefaultShopApi.Business.Support
{
    using Autofac;
    using Trooper.Thorny.Injection;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;

    public class ShopInjection
    {
        public static void AddShop(ContainerBuilder builder)
        {
            BusinessOperationInjection.AddBusinessCore<Shop, IShop>(builder);
        }
    }
}
