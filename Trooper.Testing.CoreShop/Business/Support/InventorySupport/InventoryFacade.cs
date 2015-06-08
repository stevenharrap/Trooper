using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;
using Trooper.Testing.ShopModel.Interface;
using Trooper.Testing.ShopModel.Model;
using Trooper.Thorny.Interface;
using Trooper.Thorny.Interface.DataManager;

namespace Trooper.Testing.CustomShopApi.Business.Support.InventorySupport
{
    public class InventoryFacade : Facade<Inventory, IInventory>, IInventoryFacade
    {
        public override IEnumerable<Inventory> GetSome(ISearch search)
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
