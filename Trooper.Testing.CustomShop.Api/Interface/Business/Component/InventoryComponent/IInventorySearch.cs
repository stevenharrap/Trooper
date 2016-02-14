namespace Trooper.Testing.CustomShop.Api.Interface.Business.Support.InventoryComponent
{
    using Thorny.Interface.DataManager;

    public interface IInventorySearch : ISearch
    {
        int ShopId { get; set; }
    }
}
