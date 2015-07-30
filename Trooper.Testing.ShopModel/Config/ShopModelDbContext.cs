namespace Trooper.Testing.ShopModel.Config
{
    using System.Data.Entity;
    using Trooper.Thorny.Interface.DataManager;
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
            Database.CreateIfNotExists();

            if (!Database.CompatibleWithModel(false))
            {
                Database.Delete();
                Database.Create();
            }

            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ShopModelDbContext>());
        }

        public DbSet<ShopEnt> Shops { get; set; }
        
        public DbSet<InventoryEnt> Inventories { get; set; }

        public DbSet<ProductEnt> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ShopMap());
            modelBuilder.Configurations.Add(new InventoryMap());
            modelBuilder.Configurations.Add(new ProductMap());
        }
    }
}
