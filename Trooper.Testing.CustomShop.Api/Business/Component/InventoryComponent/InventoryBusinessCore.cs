namespace Trooper.Testing.CustomShop.Api.Business.Support.InventoryComponent
{
    using Thorny.Business.Operation.Core;
    using ShopPoco;
    using ShopModel.Model;
    using Interface.Business.Support.InventoryComponent;

    public class InventoryBusinessCore : BusinessCore<InventoryEnt, Inventory>, IInventoryBusinessCore
    {
    }
}
