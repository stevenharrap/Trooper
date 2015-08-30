namespace Trooper.Testing.CustomShopApi.Business.Operation
{
    using Trooper.Thorny.Business.Operation.Composite;
    using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Trooper.Testing.CustomShopApi.Business.Model;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.OutletSupport;

    public class OutletBo : BusinessAll<OutletEnt, Outlet>, IOutletBo
    {
        public ISaveResponse<ProductInOutlet> SaveProduct(ProductInOutlet productInOutlet, IIdentity identity)
        {
            var bc = this.BusinessCore as IOutletBusinessCore;

            return bc.SaveProduct(productInOutlet, identity);
        }

        public IAddResponse<Outlet> SimpleLittleThing(IIdentity identity)
        {
            var bc = this.BusinessCore as IOutletBusinessCore;

            return bc.SimpleLittleThing(identity);
        }
    }
}
