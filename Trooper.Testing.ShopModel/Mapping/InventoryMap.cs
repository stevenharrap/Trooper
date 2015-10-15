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
            this.HasKey(t => new { t.ShopId, t.ProductId });

            // Properties
            this.Property(t => t.ShopId)
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
            this.HasRequired(t => t.Shop)
                .WithMany(t => t.Inventories)
                .HasForeignKey(d => d.ShopId);
        }
    }
}
