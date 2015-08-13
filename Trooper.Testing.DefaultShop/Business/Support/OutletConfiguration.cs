namespace Trooper.Testing.DefaultShopApi.Business.Support
{
    using Autofac;
    using Trooper.Thorny.Configuration;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;

    public class OutletConfiguration
    {
        public static void AddOutlet(ContainerBuilder builder)
        {
            var component = new BusinessComponent<OutletEnt, Outlet>(builder);
            BusinessModule.AddComponent(component);
        }
    }
}
