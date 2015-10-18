namespace Trooper.Testing.CustomShop.Api.Business.Support.InventorySupport
{
    using Thorny.DataManager;
    using ShopModel.Model;
    using Interface.Business.Support.InventorySupport;

    public class InventoryValidation : Validation<InventoryEnt>, IInventoryValidation
    {
    }
}
