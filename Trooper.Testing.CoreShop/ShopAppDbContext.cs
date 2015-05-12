namespace Trooper.Testing.CustomShopApi
{
    using System.Data.Entity;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Config;

    public class ShopAppDbContext : ShopModelDbContext
    {
        public ShopAppDbContext()
        {
        }

        public ShopAppDbContext(string context)
            : base(context)
        {
        }
    }
}
