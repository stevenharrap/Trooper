namespace Trooper.Testing.DefaultShopApi
{
    using Autofac;
    using Trooper.Thorny.Configuration;
    using Trooper.Testing.DefaultShopApi.Business.Support;

    public class ShopAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            BusinessModule.AddContext<ShopAppDbContext>(builder);

            //BusinessModuleBuilder.Initiate<ShopAppDbContext>(builder);

            ShopConfiguration.AddShop(builder);
            ProductConfiguration.AddProduct(builder);
        }
    }
}
