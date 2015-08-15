namespace Trooper.Testing.DefaultShopApi.Business.Support
{
    using Autofac;
    using Trooper.Thorny.Configuration;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;

    public class ProductConfiguration
    {
        public static void AddProduct(ContainerBuilder builder)
        {
            var component = new BusinessComponent<ProductEnt, Product>(builder);
            BusinessModule.AddComponent(component);

            //BusinessModuleBuilder.AddBusinessCore<ProductEnt, Product>(builder);
        }
    }
}
