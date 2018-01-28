using System.Collections.Generic;

namespace Iris.ViewModels
{
    #region FooterViewModel
    public class FooterViewModel
    {
        #region Properties
        public IList<LinkViewModel> PageLinks { get; set; }
        public IList<LinkViewModel> PostCategoryLinks { get; set; }
        #endregion
    }
    #endregion
}
