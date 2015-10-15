namespace Trooper.Testing.CustomShop.Api.Business.Operation
{
    using ShopPoco;
    using ShopModel.Model;
    using Thorny.Business.Operation.Single;
    using Interface.Business.Operation;

    public class ProductBo : BusinessRead<ProductEnt, Product>, IProductBo
    {
    }
}
