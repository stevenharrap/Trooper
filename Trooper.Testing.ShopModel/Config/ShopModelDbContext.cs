namespace Trooper.Testing.ShopModel.Config
{
    using System.Data.Entity;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Mapping;
    using Trooper.Testing.ShopModel.Model;
    
    public class ShopModelDbContext : DbContext, IDbContext
    {
        public ShopModelDbContext()
        {
        }

        public ShopModelDbContext(string context)
            : base(context)
        {
           // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ShopModelDbContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ShopModelDbContext, RecreateDbConfiguration>(context));
           // Database.Delete();
           //Database.Create();
        }


        public DbSet<Shop> Shops { get; set; }
        
        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ShopMap());
            modelBuilder.Configurations.Add(new InventoryMap());
            modelBuilder.Configurations.Add(new ProductMap());
        }
    }
}
