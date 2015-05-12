using System.Collections.Generic;
using Trooper.Testing.ShopModel.Interface;

namespace Trooper.Testing.ShopModel.Model
{
    public class Product : IProduct
    {
        //public int? SpecDocId { get; set; }
        
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Colour { get; set; }

        //public virtual DocNav SpecDocNav { get; set; }

        //public virtual ICollection<Inventory> Inventories { get; set; }
    }
}
