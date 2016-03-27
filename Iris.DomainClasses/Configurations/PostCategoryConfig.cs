using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Iris.DomainClasses.Configurations
{
    public class PostCategoryConfig : EntityTypeConfiguration<PostCategory>
    {
        public PostCategoryConfig()
        {
            Property(entity => entity.Name).HasMaxLength(200).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("Ix_CategoryName", 1) { IsUnique = false }));
        }
    }
}
