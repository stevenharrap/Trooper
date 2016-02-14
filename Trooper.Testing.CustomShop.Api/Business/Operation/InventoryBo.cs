namespace Trooper.Testing.CustomShop.Api.Business.Operation
{
    using ShopPoco;
    using ShopModel.Model;
    using Thorny.Business.Operation.Single;
    using Interface.Business.Operation;
    using Thorny.Business.Operation.Composite;

    public class InventoryBo : BusinessCr<InventoryEnt, Inventory>, IInventoryBo
    {
    }
}
