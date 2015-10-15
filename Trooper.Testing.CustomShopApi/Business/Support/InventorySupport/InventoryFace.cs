namespace Trooper.Testing.CustomShop.Api.Business.Support.InventorySupport
{
    using ShopPoco;
    using ShopModel.Model;
    using Thorny.Interface;
    using Interface.Business.Support.InventorySupport;

    public class InventoryFace : Facade<InventoryEnt, Inventory>,  IInventoryFacade
    {
    }
}
