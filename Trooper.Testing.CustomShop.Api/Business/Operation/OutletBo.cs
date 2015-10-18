namespace Trooper.Testing.CustomShop.Api.Business.Operation
{
    using Thorny.Business.Operation.Composite;
    using ShopPoco;
    using ShopModel.Model;
    using Interface.Business.Operation;

    public class OutletBo : BusinessAll<OutletEnt, Outlet>, IOutletBo
    {
        //public ISaveResponse<ProductInOutlet> SaveProduct(ProductInOutlet productInOutlet, IIdentity identity)
        //{
        //    var bc = this.BusinessCore as IOutletBusinessCore;

        //    return bc.SaveProduct(productInOutlet, identity);
        //}

        //public IAddResponse<Outlet> SimpleLittleThing(IIdentity identity)
        //{
        //    var bc = this.BusinessCore as IOutletBusinessCore;
            
        //    return bc.SimpleLittleThing(identity);
        //}
    }
}
