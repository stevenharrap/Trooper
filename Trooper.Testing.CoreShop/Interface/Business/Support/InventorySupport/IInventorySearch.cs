namespace Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport
{
    using Trooper.Thorny.Interface.DataManager;

    public interface IInventorySearch : ISearch
    {
        int ShopId { get; set; }
    }
}
