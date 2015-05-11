using Trooper.Testing.ShopModel.Interface;

namespace Trooper.Testing.ShopModel
{
    public class Shop : IShop
    {
        public int ShopId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }      
    }
}
