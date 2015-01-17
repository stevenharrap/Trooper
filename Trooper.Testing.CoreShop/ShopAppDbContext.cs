namespace Trooper.Testing.CoreShop
{
    using System.Data.Entity;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.Testing.CoreShop.Model;

    public class ShopAppDbContext : DbContext, IDbContext
    {
        static ShopAppDbContext()
        {
            //Database.SetInitializer<ShopAppDbContext>(new TestContextInitializer());
        }

        public ShopAppDbContext()
            : this("TrooperUnitTestingDbContext")
        {

        }
        
        public ShopAppDbContext(string context)
            : base(context)
        {
            Database.Delete();
            Database.Create();
        }


        public DbSet<Shop> Shops { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ShopMap());
        }
    }
}
