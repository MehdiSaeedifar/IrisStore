using System.Collections.Generic;

namespace Iris.ViewModels
{
    public class HomePageViewModel
    {
        public IList<ProductWidgetViewModel> NewestProducts { get; set; }
        public IList<ProductWidgetViewModel> MostViewedProducts { get; set; }
        public IList<ProductWidgetViewModel> PopularProducts { get; set; }
        public IList<SidebarCategoryViewModel> Categories { get; set; }
        public IList<SlideShowViewModel> SlideShows { get; set; }
        public IList<LinkViewModel> PageLinks { get; set; }
        public IList<LinkViewModel> PostCategoryLinks { get; set; }
        public IList<PostCategorySideBarViewModel> PostCategories { get; set; }
    }
}
