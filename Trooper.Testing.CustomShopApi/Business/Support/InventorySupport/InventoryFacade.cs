namespace Trooper.Testing.CustomShop.Api.Business.Support.InventorySupport
{
    using System.Collections.Generic;
    using System.Linq;
    using ShopPoco;
    using ShopModel.Model;
    using Thorny.Interface;
    using Thorny.Interface.DataManager;
    using Interface.Business.Support.InventorySupport;

    public class InventoryFacade : Facade<InventoryEnt, Inventory>, IInventoryFacade
    {
        public override IEnumerable<InventoryEnt> GetSome(ISearch search)
        {
            var inventorySearch = search as IInventorySearch;

            if (inventorySearch == null)
            {
                return base.GetSome(search);
            }

            return from i in this.GetAll()
                   where i.ShopId == inventorySearch.ShopId
                   select i;            
        }
    }
}
