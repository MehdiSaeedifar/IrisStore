using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Iris.ServiceLayer
{
    public class ApplicationSignInManager :
        SignInManager<ApplicationUser, int>, IApplicationSignInManager
    {
        #region Feilds
        private readonly ApplicationUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;
        #endregion

        #region Constractors
        public ApplicationSignInManager(ApplicationUserManager userManager,
                                        IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }
        #endregion
    }
}