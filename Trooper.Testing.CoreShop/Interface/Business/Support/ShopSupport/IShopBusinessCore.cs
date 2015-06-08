using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;
using Trooper.Testing.CustomShopApi.Interface.Business.Model;
using Trooper.Testing.ShopModel;
using Trooper.Testing.ShopModel.Interface;
using Trooper.Testing.ShopModel.Model;

namespace Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport
{
    public interface IShopBusinessCore : IBusinessCore<Shop, IShop>
    {
        ISaveResponse<IProductInShop> SaveProduct(IProductInShop productInShop, IIdentity identity);

        IManyResponse<IProductInShop> GetProducts(IShop shop, IIdentity identity);
    }
}
