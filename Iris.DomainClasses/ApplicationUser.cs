using Microsoft.AspNet.Identity.EntityFramework;

namespace Iris.DomainClasses
{
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {

    }
}