namespace Trooper.Testing.CustomShop.Api.Business.Support.InventoryComponent
{
    using ShopPoco;
    using ShopModel.Model;
    using Thorny.Interface;
    using Interface.Business.Support.InventoryComponent;

    public class InventoryFace : Facade<InventoryEnt, Inventory>,  IInventoryFacade
    {
    }
}
