using System.Collections.Generic;

namespace Iris.Web.ViewModels.Identity
{
    #region SendCodeViewModel
    public class SendCodeViewModel
    {
        #region Properties
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        #endregion
    }
    #endregion
}