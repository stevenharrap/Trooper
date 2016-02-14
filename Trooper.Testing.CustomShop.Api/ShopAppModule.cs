namespace Trooper.Testing.CustomShopApi
{
    using Autofac;
    using Thorny.Configuration;
    using CustomShop.Api.Business.Support.OutletComponent;
    using CustomShop.Api.Business.Support.InventoryComponent;
    using CustomShop.Api.Business.Support.ProductComponent;

    public class ShopAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            BusinessModule.AddContext<ShopAppDbContext>(builder);

            OutletConfiguration.AddShop(builder);
            ProductConfiguration.AddProduct(builder);
            InventoryConfiguration.AddInventory(builder);            
        }
    }
}
