using System.Collections.Generic;

namespace Iris.ViewModels
{
    public class FooterViewModel
    {
        public IList<LinkViewModel> PageLinks { get; set; }
        public IList<LinkViewModel> PostCategoryLinks { get; set; }
        public string FooterDiscription { get; set; }
    }
}
