namespace Trooper.Testing.CustomShop.Api.Business.Support.InventorySupport
{
    using Interface.Business.Support.InventorySupport;
    using Trooper.Thorny.Business.Operation.Core;

    public class InventorySearch : Search, IInventorySearch
    {
        public int ShopId { get; set; }
    }
}
