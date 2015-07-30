using System.Collections.Generic;
using Trooper.Testing.ShopModel.Poco;

namespace Trooper.Testing.ShopModel.Model
{
    public class ProductEnt : Product
    {
        public virtual ICollection<InventoryEnt> Inventories { get; set; }
    }
}
