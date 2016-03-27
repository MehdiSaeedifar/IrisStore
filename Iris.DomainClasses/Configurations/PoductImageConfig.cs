using System.Data.Entity.ModelConfiguration;

namespace Iris.DomainClasses.Configurations
{
    public class PoductImageConfig : EntityTypeConfiguration<ProductImage>
    {
        public PoductImageConfig()
        {
            Property(entity => entity.Name).HasMaxLength(200);

            Property(entity => entity.DeleteUrl).HasMaxLength(500);

            Property(entity => entity.ThumbnailUrl).HasMaxLength(500);

            Property(entity => entity.Url).HasMaxLength(500);
        }
    }
}
