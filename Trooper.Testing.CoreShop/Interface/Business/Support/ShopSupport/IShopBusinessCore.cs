using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;
using Trooper.Testing.ShopModel;
using Trooper.Testing.ShopModel.Poco;
using Trooper.Testing.ShopModel.Model;
using Trooper.Testing.CustomShopApi.Business.Model;

namespace Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport
{
    public interface IShopBusinessCore : IBusinessCore<ShopEnt, Shop>
    {
        ISaveResponse<ProductInShop> SaveProduct(ProductInShop productInShop, IIdentity identity);

        IManyResponse<ProductInShop> GetProducts(Shop shop, IIdentity identity);
    }
}
