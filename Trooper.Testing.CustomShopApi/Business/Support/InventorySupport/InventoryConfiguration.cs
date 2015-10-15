namespace Trooper.Testing.CustomShop.Api.Business.Support.InventorySupport
{
    using Autofac;
    using ShopModel.Model;
    using ShopPoco;
    using Thorny.Configuration;
    using Interface.Business.Support.InventorySupport;
    using Operation;
    using Interface.Business.Operation;

    public class InventoryConfiguration
    {
        public static void AddInventory(ContainerBuilder builder)
        {
            var component = new BusinessComponent<InventoryEnt, Inventory>(builder);
            component.RegisterFacade<InventoryFacade, IInventoryFacade>();
            component.RegisterAuthorization<InventoryAuthorization, IInventoryAuthorization>();
            component.RegisterValidation<InventoryValidation, IInventoryValidation>();
            component.RegisterBusinessCore<InventoryBusinessCore, IInventoryBusinessCore>();
            component.RegisterBusinessOperation<InventoryBo, IInventoryBo>();

            component.RegisterDynamicServiceHost(new BusinessHostInfo { BaseAddress = "http://localhost:8000" });

            BusinessModule.AddComponent(component);
        }
    }
}
