namespace Trooper.Testing.DefaultShopApi.Business.Support
{
    using Autofac;
    using Trooper.Thorny.Configuration;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;

    public class ShopConfiguration
    {
        public static void AddShop(ContainerBuilder builder)
        {
            var component = new BusinessComponent<ShopEnt, Shop>(builder);
            BusinessModule.AddComponent(component);
        }
    }
}
