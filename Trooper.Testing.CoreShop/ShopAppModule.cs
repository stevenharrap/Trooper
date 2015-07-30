namespace Trooper.Testing.CustomShopApi
{
    using Autofac;
    using Trooper.Thorny.Configuration;
    using Trooper.Testing.CustomShopApi.Business.Support;
    using Trooper.Testing.CustomShopApi.Business.Support.ShopSupport;
    using Trooper.Testing.CustomShopApi.Business.Support.InventorySupport;

    public class ShopAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //BusinessModuleBuilder.Initiate<ShopAppDbContext>(builder);

            BusinessModule.AddContext<ShopAppDbContext>(builder);
            //BusinessModule.AddStandardRegistrations(builder);

            ShopConfiguration.AddShop(builder);
            ProductConfiguration.AddProduct(builder);
            InventoryConfiguration.AddInventory(builder);            
        }
    }
}
