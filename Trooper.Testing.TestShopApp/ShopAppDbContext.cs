namespace Trooper.Testing.DefaultShopApp
{
    public class ShopAppDbContext : DefaultShopApi.ShopAppDbContext
    {
        public ShopAppDbContext()
            : base("TrooperUnitTestingDbContext")
        {
        }

    }
}
