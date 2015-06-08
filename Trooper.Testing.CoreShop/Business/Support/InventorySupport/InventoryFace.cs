namespace Trooper.Testing.CustomShopApi.Business.Support.InventorySupport
{
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Thorny.Interface;

    public class InventoryFace : Facade<Inventory, IInventory>,  IInventoryFacade
    {
    }
}
