namespace Trooper.Testing.CoreShop
{
    using System.Data.Entity;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.Testing.CoreShop.Model;

    public class ShopAppDbContext : DbContext, IDbContext
    {
        public ShopAppDbContext()
            : this("TrooperUnitTestingDbContext")
        {
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ShopAppDbContext>());
			
        }
        
        public ShopAppDbContext(string context)
            : base(context)
        {
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ShopAppDbContext>());
           // Database.Delete();
            //Database.Create();
        }


        public DbSet<Shop> Shops { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ShopMap());
        }
    }
}
