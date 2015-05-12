using System.Collections.Generic;
using Trooper.Testing.ShopModel.Interface;

namespace Trooper.Testing.ShopModel.Model
{
    public class Shop : IShop
    {
        public int ShopId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
    }
}
