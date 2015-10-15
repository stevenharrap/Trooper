namespace Trooper.Testing.CustomShop.Api.Business.Support.InventorySupport
{
    using Thorny.Business.Operation.Core;
    using ShopPoco;
    using ShopModel.Model;
    using Interface.Business.Support.InventorySupport;

    public class InventoryBusinessCore : BusinessCore<InventoryEnt, Inventory>, IInventoryBusinessCore
    {
    }
}
