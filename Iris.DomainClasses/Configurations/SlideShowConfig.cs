using System.Data.Entity.ModelConfiguration;

namespace Iris.DomainClasses.Configurations
{
    public class SlideShowConfig : EntityTypeConfiguration<SlideShowImage>
    {
        public SlideShowConfig()
        {
            Property(entity => entity.Image).HasMaxLength(600);

            Property(entity => entity.Link).HasMaxLength(800);

            Property(entity => entity.Title).HasMaxLength(300);

            Property(entity => entity.Description).HasMaxLength(600);
        }
    }
}
