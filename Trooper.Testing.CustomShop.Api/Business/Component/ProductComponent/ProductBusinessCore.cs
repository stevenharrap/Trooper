namespace Trooper.Testing.CustomShop.Api.Business.Support.ProductComponent
{
    using Thorny.Business.Operation.Core;
    using ShopPoco;
    using ShopModel.Model;
    using Interface.Business.Support.ProductComponent;

    public class ProductBusinessCore : BusinessCore<ProductEnt, Product>, IProductBusinessCore
    {
    }
}
