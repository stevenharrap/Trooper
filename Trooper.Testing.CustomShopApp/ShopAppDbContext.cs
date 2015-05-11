using Trooper.Testing.CustomShopApi;

namespace Trooper.Testing.CustomShopApp
{
    public class ShopAppDbContext : CustomShopApi.ShopAppDbContext
    {
        public ShopAppDbContext() : base("TrooperCustomShopTesting")
        {
        }

    }
}
