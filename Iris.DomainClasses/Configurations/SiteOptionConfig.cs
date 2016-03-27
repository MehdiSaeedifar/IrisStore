using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Iris.DomainClasses.Configurations
{
    public class SiteOptionConfig : EntityTypeConfiguration<SiteOption>
    {
        public SiteOptionConfig()
        {
            Property(entity => entity.Key).HasMaxLength(50).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("Ix_CategoryName", 1) { IsUnique = true }));

            Property(entity => entity.Value).HasMaxLength(600);
        }
    }
}
