namespace Trooper.Testing.CoreShop
{
    using Autofac;
    using Trooper.BusinessOperation2.Injection;
    using Trooper.Testing.CoreShop.Business.Support;

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
