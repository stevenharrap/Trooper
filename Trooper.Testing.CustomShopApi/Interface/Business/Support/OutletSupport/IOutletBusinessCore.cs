using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;
using Trooper.Testing.ShopModel;
using Trooper.Testing.ShopModel.Poco;
using Trooper.Testing.ShopModel.Model;
using Trooper.Testing.CustomShopApi.Business.Model;

namespace Trooper.Testing.CustomShopApi.Interface.Business.Support.OutletSupport
{
    public interface IOutletBusinessCore : IBusinessCore<OutletEnt, Outlet>
    {
        //ISaveResponse<ProductInOutlet> SaveProduct(ProductInOutlet productInShop, IIdentity identity);

        //IManyResponse<ProductInOutlet> GetProducts(Outlet outlet, IIdentity identity);

        //IAddResponse<Outlet> SimpleLittleThing(IIdentity identity);
    }
}
