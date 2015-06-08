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

            BusinessModuleBuilder.AddUnitOfWork<ShopAppDbContext>(builder);

            ShopInjection.AddShop(builder);
            ProductInjection.AddProduct(builder);
        }
    }
}
