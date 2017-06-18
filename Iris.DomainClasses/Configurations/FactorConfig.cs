using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses.Configurations
{
    public class FactorConfig : EntityTypeConfiguration<Factor>
    {
        public FactorConfig()
        {
            HasRequired(f=>f.User)
                .WithMany(u=>u.Factors)
                .HasForeignKey(f=>f.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
