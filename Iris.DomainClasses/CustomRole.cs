using Microsoft.AspNet.Identity.EntityFramework;

namespace Iris.DomainClasses
{
    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        #region Properties
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
        #endregion
    }
}