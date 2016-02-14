namespace Trooper.Testing.CustomShop.Api.Business.Operation
{
    using ShopPoco;
    using ShopModel.Model;
    using Thorny.Business.Operation.Single;
    using Interface.Business.Operation;
    using Thorny.Business.Operation.Composite;

    public class ProductBo : BusinessCr<ProductEnt, Product>, IProductBo
    {
    }
}
