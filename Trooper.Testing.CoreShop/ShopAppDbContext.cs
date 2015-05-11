namespace Trooper.Testing.CustomShopApi
{
    using System.Data.Entity;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.Testing.ShopModel;
    
    public class ShopAppDbContext : DbContext, IDbContext
    {
        public ShopAppDbContext()
        {
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
