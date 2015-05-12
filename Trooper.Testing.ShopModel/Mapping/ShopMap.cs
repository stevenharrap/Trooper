namespace Trooper.Testing.ShopModel.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Trooper.Testing.ShopModel.Model;

    public class ShopMap : EntityTypeConfiguration<Shop>
    {
        public ShopMap()
        {
            this.ToTable("Shop");
                        
            this.HasKey(t => t.ShopId);
            this.Property(t => t.ShopId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name).HasMaxLength(50).IsOptional();

            this.Property(t => t.Address).HasMaxLength(50).IsOptional();
        }
    }
}
