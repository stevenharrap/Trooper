namespace Trooper.Testing.CustomShopApi.Business.Operation
{
    using Trooper.Thorny.Business.Operation.Composite;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Thorny.Business.Operation.Single;

    public class ProductBo : BusinessRead<Product, IProduct>, IProductBo
    {
    }
}
