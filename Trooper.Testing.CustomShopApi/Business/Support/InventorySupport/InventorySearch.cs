using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;
using Trooper.Thorny.Business.Operation.Core;

namespace Trooper.Testing.CustomShopApi.Business.Support.InventorySupport
{
    public class InventorySearch : Search, IInventorySearch
    {
        public int ShopId { get; set; }
    }
}
