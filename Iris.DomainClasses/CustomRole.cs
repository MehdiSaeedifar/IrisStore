using Microsoft.AspNet.Identity.EntityFramework;

namespace Iris.DomainClasses
{
    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }


    }
}