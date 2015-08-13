using System.Collections.Generic;
using Trooper.Testing.ShopModel.Poco;

namespace Trooper.Testing.ShopModel.Model
{
    public class OutletEnt : Outlet
    {
        public virtual ICollection<InventoryEnt> Inventories { get; set; }
    }
}
