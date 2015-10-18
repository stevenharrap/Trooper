namespace Trooper.Testing.CustomShop.Api.Interface.Business.Support.OutletSupport
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using ShopPoco;
    using ShopModel.Model;

    public interface IOutletBusinessCore : IBusinessCore<OutletEnt, Outlet>
    {
        //ISaveResponse<ProductInOutlet> SaveProduct(ProductInOutlet productInShop, IIdentity identity);

        //IManyResponse<ProductInOutlet> GetProducts(Outlet outlet, IIdentity identity);

        //IAddResponse<Outlet> SimpleLittleThing(IIdentity identity);
    }
}
