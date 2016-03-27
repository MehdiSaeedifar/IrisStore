using System.Data.Entity.ModelConfiguration;

namespace Iris.DomainClasses.Configurations
{
    public class ProductPriceConfig : EntityTypeConfiguration<ProductPrice>
    {
        public ProductPriceConfig()
        {
            HasRequired(price => price.Product)
            .WithMany(product => product.Prices)
            .HasForeignKey(price => price.ProductId)
            .WillCascadeOnDelete(true);
        }
    }
}
