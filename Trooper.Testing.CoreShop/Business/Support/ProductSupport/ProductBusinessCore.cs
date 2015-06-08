using Trooper.Thorny.Business.Operation.Core;
using Trooper.Testing.ShopModel.Interface;
using Trooper.Testing.ShopModel.Model;
using Trooper.Testing.CustomShopApi.Interface.Business.Support.ProductSupport;

namespace Trooper.Testing.CustomShopApi.Business.Support.ShopSupport
{
    public class ProductBusinessCore : BusinessCore<Product, IProduct>, IProductBusinessCore
    {
    }
}
