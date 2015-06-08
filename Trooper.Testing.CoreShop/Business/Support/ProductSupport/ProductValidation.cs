namespace Trooper.Testing.CustomShopApi.Business.Support.ShopSupport
{
    using Trooper.Thorny.DataManager;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport;

    public class ProductValidation : Validation<Product>, IProductValidation
    {
    }
}
