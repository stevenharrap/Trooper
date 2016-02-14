namespace Trooper.Testing.ShopModel.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Model;

    public class InventoryMap : EntityTypeConfiguration<InventoryEnt>
    {
        public InventoryMap()
        {
            // Primary Key          
            this.HasKey(t => new { t.OutletIdId, t.ProductId });

            // Properties
            this.Property(t => t.OutletIdId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProductId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Bin).HasMaxLength(10).IsOptional();

            this.Property(t => t.Quantity).IsRequired();

            // Table & Column Mappings
            this.ToTable("Inventory");

            // Relationships
            this.HasRequired(t => t.Product)
                .WithMany(t => t.Inventories)
                .HasForeignKey(d => d.ProductId);
            this.HasRequired(t => t.Outlet)
                .WithMany(t => t.Inventories)
                .HasForeignKey(d => d.OutletIdId);
        }
    }
}
