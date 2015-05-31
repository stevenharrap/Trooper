using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Testing.CustomShopApi.Interface.Business.Model;

namespace Trooper.Testing.CustomShopApi.Business.Model
{
    public class ProductInShop : IProductInShop
    {
        public int Quantity { get; set; }

        public int ShopId { get; set; }

        public string Name { get; set; }

        public int ProductId { get; set; }

        public string Colour { get; set; }
    }
}
