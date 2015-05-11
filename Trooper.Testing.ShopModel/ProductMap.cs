namespace Shop.Model.Map
{
    using System.Data.Entity.ModelConfiguration;



    public class ProductMap //: EntityTypeConfiguration<ProductNav>
    {
        public ProductMap()
        {
            // Primary Key
            //this.HasKey(t => t.ProductId);

            // Table & Column Mappings
           // this.ToTable("Product");

            /*this.HasOptional(t => t.SpecDocNav)
                .WithMany(t => t.SpecDocNavs)
                .HasForeignKey(d => d.SpecDocId);*/
        }
    }
}
