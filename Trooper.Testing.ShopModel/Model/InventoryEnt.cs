using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Testing.ShopPoco;

namespace Trooper.Testing.ShopModel.Model
{
    public class InventoryEnt : Inventory
    {
        public virtual ProductEnt Product { get; set; }

        public virtual OutletEnt Shop { get; set; }
    }
}
