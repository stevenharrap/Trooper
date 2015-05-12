using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Testing.ShopModel.Interface;

namespace Trooper.Testing.ShopModel.Model
{
    public class Inventory : IInventory
    {
        public int ShopId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Bin { get; set; }

        //public virtual Product Product { get; set; }

        //public virtual Shop Shop { get; set; }
    }
}
