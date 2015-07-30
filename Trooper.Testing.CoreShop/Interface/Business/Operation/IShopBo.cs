using System.ServiceModel;
using Trooper.Interface.Thorny.Business.Operation.Composite;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;
using Trooper.Testing.ShopModel.Poco;
using Trooper.Testing.ShopModel.Model;
using Trooper.Testing.CustomShopApi.Business.Model;

namespace Trooper.Testing.CustomShopApi.Interface.Business.Operation
{
    [ServiceContract]
    public interface IShopBo : IBusinessCr<ShopEnt, Shop>
    {
        [OperationContract]
        ISingleResponse<Product> SaveProduct(ProductInShop productInShop, IIdentity identity);
    }
}
