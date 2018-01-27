using System.Data.Entity.ModelConfiguration;

namespace Iris.DomainClasses.Configurations
{
    public class ProductConfig : EntityTypeConfiguration<Product>
    {
        public ProductConfig()
        {
            //HasMany(product => product.Prices)
            //    .WithRequired(price => price.Product)
            //    .HasForeignKey(price => price.ProductId)
            //    .WillCascadeOnDelete(true);

            //HasMany(product => product.Discounts)
            //  .WithRequired(discount => discount.Product)
            //  .HasForeignKey(discount => discount.ProductId)
            //  .WillCascadeOnDelete(true);

            HasMany(product => product.Images)
                .WithRequired(image => image.Product)
                .HasForeignKey(image => image.ProductId)
                .WillCascadeOnDelete(true);

            Property(entity => entity.Title).HasMaxLength(500);

            Property(entity => entity.Body).IsMaxLength();

            Property(entity => entity.MetaDescription).HasMaxLength(400);

            Property(entity => entity.SlugUrl).HasMaxLength(300);
        }
    }
}
