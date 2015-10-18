namespace Trooper.Testing.CustomShop.Api.Business.Support.ProductSupport
{
    using ShopModel.Model;
    using Thorny.DataManager;
    using Interface.Business.Support.OutletSupport;

    public class ProductValidation : Validation<ProductEnt>, IProductValidation
    {
    }
}
