namespace Trooper.Testing.ShopModel.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Trooper.Testing.ShopModel.Model;

    public class OutletMap : EntityTypeConfiguration<OutletEnt>
    {
        public OutletMap()
        {
            this.ToTable("Outlet");
                        
            this.HasKey(t => t.OutletId);
            this.Property(t => t.OutletId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name).HasMaxLength(50).IsOptional();

            this.Property(t => t.Address).HasMaxLength(50).IsOptional();
        }
    }
}
