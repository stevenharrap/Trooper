using System.Collections.Generic;
using Trooper.Testing.ShopPoco;

namespace Trooper.Testing.ShopModel.Model
{
    public class ProductEnt : Product
    {
        public virtual ICollection<InventoryEnt> Inventories { get; set; }
    }
}
