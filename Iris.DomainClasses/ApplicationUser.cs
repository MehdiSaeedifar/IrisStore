using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Iris.DomainClasses
{
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public virtual ICollection<Factor> Factors { get; set; }
    }
}