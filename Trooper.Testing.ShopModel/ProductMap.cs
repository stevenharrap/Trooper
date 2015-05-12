namespace Trooper.Testing.ShopModel
{
    using System.Data.Entity.ModelConfiguration;

    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductId);

            // Table & Column Mappings
            this.ToTable("Product");

            this.Property(t => t.Name).HasMaxLength(30).IsRequired();

            /*this.HasOptional(t => t.SpecDocNav)
                .WithMany(t => t.SpecDocNavs)
                .HasForeignKey(d => d.SpecDocId);*/
        }
    }
}
