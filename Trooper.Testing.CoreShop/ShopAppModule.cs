namespace Trooper.Testing.CustomShopApi
{
    using Autofac;
    using Trooper.Thorny.Injection;
    using Trooper.Testing.CustomShopApi.Business.Support;

    public class ShopAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            BusinessOperationInjection.AddUnitOfWork<ShopAppDbContext>(builder);

            ShopInjection.AddShop(builder);
        }
    }
}
