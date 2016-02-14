namespace Trooper.Testing.CustomShop.Api.Business.Support.OutletComponent
{
    using ShopModel.Model;
    using ShopPoco;
    using Thorny.Business.Operation.Core;
    using Interface.Business.Support.OutletComponent;

    public class OutletCache : SimpleCache<OutletEnt, Outlet>, IOutletCache
    {
    }
}
