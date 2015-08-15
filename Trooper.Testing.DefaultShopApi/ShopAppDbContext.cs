namespace Trooper.Testing.DefaultShopApi
{
    using System.Data.Entity;
    using Trooper.Thorny.Interface.DataManager;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Config;

    public class ShopAppDbContext : ShopModelDbContext
    {
        public ShopAppDbContext()
            : base("TrooperUnitTestingDbContext")
        {

        }
    }
}