using System.Data.Entity.ModelConfiguration;

namespace Iris.DomainClasses.Configurations
{
    public class ProductDiscountConfig : EntityTypeConfiguration<ProductDiscount>
    {
        public ProductDiscountConfig()
        {
            HasRequired(discount => discount.Product)
            .WithMany(product => product.Discounts)
            .HasForeignKey(discount => discount.ProductId)
            .WillCascadeOnDelete(true);
        }
    }
}
