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
    public interface IOutletBo : IBusinessCr<OutletEnt, Outlet>
    {
        [OperationContract]
        ISaveResponse<ProductInOutlet> SaveProduct(ProductInOutlet productInShop, IIdentity identity);

        [OperationContract]
        IAddResponse<Outlet> SimpleLittleThing(IIdentity identity);
    }
}
