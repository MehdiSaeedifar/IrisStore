using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Iris.Web.ViewModels.Identity
{
    #region ManageLoginsViewModel
    public class ManageLoginsViewModel
    {
        #region Properties
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
        #endregion
    }
    #endregion
}