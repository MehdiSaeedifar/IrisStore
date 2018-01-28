using System.Collections.Generic;
using Iris.ViewModels;

namespace Iris.Web.ViewModels
{
    #region ProductSliderWidgetViewModel
    public class ProductSliderWidgetViewModel
    {
        #region Properties
        public string Title { get; set; }
        public string CarouselId { get; set; }
        public string LinkText { get; set; }
        public string Link { get; set; }
        public IList<ProductWidgetViewModel> Products { get; set; }
        #endregion
    }
    #endregion
}