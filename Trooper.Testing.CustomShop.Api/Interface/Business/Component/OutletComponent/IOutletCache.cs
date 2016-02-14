namespace Trooper.Testing.CustomShop.Api.Interface.Business.Support.OutletComponent
{
    using ShopModel.Model;
    using ShopPoco;
    using Trooper.Interface.Thorny.Business.Operation.Core;

    public interface IOutletCache : ICache<OutletEnt, Outlet>
    {        
    }
}
