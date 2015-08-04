namespace Trooper.Testing.CustomShopApi.Business.Support.InventorySupport
{
    using Autofac;
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Testing.CustomShopApi.Business.Operation;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Thorny.Business.Response;
    using Trooper.Thorny.Configuration;

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

            component.RegisterServiceHost("http://localhost:8000");

            BusinessModule.AddComponent(component);
        }
    }
}
