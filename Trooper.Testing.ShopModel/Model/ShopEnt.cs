using System.Collections.Generic;
using Trooper.Testing.ShopModel.Poco;

namespace Trooper.Testing.ShopModel.Model
{
    public class ShopEnt : Shop
    {
        public virtual ICollection<InventoryEnt> Inventories { get; set; }
    }
}
