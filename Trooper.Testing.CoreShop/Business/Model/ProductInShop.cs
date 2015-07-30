using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Testing.ShopModel.Poco;

namespace Trooper.Testing.CustomShopApi.Business.Model
{
    public class ProductInShop
    {
        public int Quantity { get; set; }

        public int ShopId { get; set; }

        public string Name { get; set; }

        public int ProductId { get; set; }

        public string Colour { get; set; }

        public string Bin { get; set; }

        public Product AsProduct()
        {
            return new Product
            {
                Colour = this.Colour,
                Name = this.Name,
                ProductId = this.ProductId
            };
        }

        public Inventory AsInventory()
        {
            return new Inventory
            {
                Bin = this.Bin,
                ProductId = this.ProductId,
                Quantity = this.Quantity,
                ShopId = this.ShopId
            };
        }
    }
}
