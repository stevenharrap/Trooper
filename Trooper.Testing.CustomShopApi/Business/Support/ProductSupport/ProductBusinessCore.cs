namespace Trooper.Testing.CustomShop.Api.Business.Support.ProductSupport
{
    using Thorny.Business.Operation.Core;
    using ShopPoco;
    using ShopModel.Model;
    using Interface.Business.Support.ProductSupport;

    public class ProductBusinessCore : BusinessCore<ProductEnt, Product>, IProductBusinessCore
    {
    }
}
