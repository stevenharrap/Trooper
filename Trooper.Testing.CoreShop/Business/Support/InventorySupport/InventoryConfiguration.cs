namespace Trooper.Testing.CustomShopApi.Business.Support.InventorySupport
{
    using Autofac;
    using Trooper.Thorny.Configuration;
    using Trooper.Testing.CustomShopApi.Business.Operation;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport;
    using Trooper.Thorny.Interface;
    using Trooper.Thorny.Interface.DataManager;
    using Trooper.Testing.CustomShopApi.Business.Support.ProductSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ProductSupport;
    using Trooper.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;

    public class InventoryConfiguration
    {
        public static void AddInventory(ContainerBuilder builder)
        {
            BusinessModuleBuilder.AddBusinessCore<
                InventoryFacade, IInventoryFacade,
                InventoryAuthorization, IInventoryAuthorization,
                InventoryValidation, IInventoryValidation,
                InventoryBusinessCore, IInventoryBusinessCore,
                InventoryBo, IInventoryBo,
                Inventory, IInventory>(builder);

            BusinessModuleBuilder.AddServiceHost<InventoryBo, IInventoryBo>(builder);

            //BusinessOperationInjection.AddBusinessOperation<IShopBusinessCore, ShopBo, IShopBo, Shop, IShop>(builder);
        }
    }
}
