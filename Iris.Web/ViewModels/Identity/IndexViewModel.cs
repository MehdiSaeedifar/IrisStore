using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Iris.Web.ViewModels.Identity
{
    #region IndexViewModel
    public class IndexViewModel
    {
        #region Properties
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        #endregion
    }
    #endregion
}