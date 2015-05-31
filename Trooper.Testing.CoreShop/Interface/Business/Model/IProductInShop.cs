using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Testing.ShopModel.Interface;

namespace Trooper.Testing.CustomShopApi.Interface.Business.Model
{
    public interface IProductInShop : IProduct
    {
        int ShopId { get; set; }

        int Quantity { get; set; }
    }
}
