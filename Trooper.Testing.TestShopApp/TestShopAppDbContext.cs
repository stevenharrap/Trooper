namespace Trooper.NUnitTesting.TestShopApp
{
    using Trooper.Testing.CoreShop;

    public class TestShopAppDbContext : ShopAppDbContext
    {
        public TestShopAppDbContext()
            : base("TrooperUnitTestingDbContext")
        {
        }

    }
}
