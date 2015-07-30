namespace Trooper.Testing.CustomShopApi.Business.Support.InventorySupport
{
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Thorny.Interface;

    public class InventoryFace : Facade<InventoryEnt, Inventory>,  IInventoryFacade
    {
    }
}
