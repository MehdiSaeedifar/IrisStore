using System.Collections.Generic;
using System.Web.Mvc;

namespace Iris.Web.ViewModels.Identity
{
    #region ConfigureTwoFactorViewModel
    public class ConfigureTwoFactorViewModel
    {
        #region Properties
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        #endregion
    }
    #endregion
}