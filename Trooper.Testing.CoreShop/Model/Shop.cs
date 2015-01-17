namespace Trooper.Testing.CoreShop.Model
{
    using Trooper.Testing.CoreShop.Interface.Model;

    public class Shop : IShop
    {
        public int ShopId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }      
    }
}
