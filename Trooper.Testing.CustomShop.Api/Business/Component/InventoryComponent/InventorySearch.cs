namespace Trooper.Testing.CustomShop.Api.Business.Support.InventoryComponent
{
    using Interface.Business.Support.InventoryComponent;
    using Trooper.Thorny.Business.Operation.Core;

    public class InventorySearch : Search, IInventorySearch
    {
        public int ShopId { get; set; }
    }
}
