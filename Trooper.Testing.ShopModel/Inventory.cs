using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Testing.ShopModel
{
    public class Inventory
    {
        public int ShopId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Bin { get; set; }

        public virtual Product ProductNav { get; set; }

        public virtual Shop ShopNav { get; set; }
    }
}
