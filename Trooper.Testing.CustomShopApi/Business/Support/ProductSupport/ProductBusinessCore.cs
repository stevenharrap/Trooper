using Trooper.Thorny.Business.Operation.Core;
using Trooper.Testing.ShopModel.Poco;
using Trooper.Testing.ShopModel.Model;
using Trooper.Testing.CustomShopApi.Interface.Business.Support.ProductSupport;

namespace Trooper.Testing.CustomShopApi.Business.Support.OutletSupport
{
    public class ProductBusinessCore : BusinessCore<ProductEnt, Product>, IProductBusinessCore
    {
    }
}
