using System.Threading.Tasks;
using System.Web.Mvc;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;

namespace Iris.Web.Controllers
{
    [RoutePrefix("Home")]
    public partial class HomeController : Controller
    {

        private IApplicationUserManager _userManager;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IPostCategoryService _postCategoryService;
        private readonly ISlideShowImageService _slideShowService;
        private readonly IPageService _pageService;
        private readonly ISiteSettingService _settingService;

        public HomeController(IApplicationUserManager userManager, IProductService productService, ICategoryService categoryService,
            ISlideShowImageService slideShowService, IPostCategoryService postCategoryService, IPageService pageService, ISiteSettingService settingService)
        {
            _userManager = userManager;
            _productService = productService;
            _categoryService = categoryService;
            _slideShowService = slideShowService;
            _postCategoryService = postCategoryService;
            _pageService = pageService;
            _settingService = settingService;
        }

        [Route("~/")]
        [Route]
        [Route("Index")]
        public virtual async Task<ActionResult> Index()
        {

            var model = new HomePageViewModel
            {
                SuggestionProducts = await _productService.GetSuggestionProducts(9),
                NewestProducts = await _productService.GetNewestProducts(8),
                MostViewedProducts = await _productService.GetMostViewedProducts(8),
                PopularProducts = await _productService.GetPopularProducts(8),
                Categories = await _categoryService.GetSidebarCategories(5),
                SlideShows = await _slideShowService.GetSlideShowImages(),
                PostCategories = await _postCategoryService.GetSideBar()
            };

            var metaTags = await _settingService.GetSiteMetaTags();

            ViewBag.Keywords = metaTags.Keywords;
            ViewBag.MetaDescription = metaTags.Description;

            return View(model);
        }

        [Route("MenuBar")]
        public virtual async Task<ActionResult> MenuBar()
        {
            var pageLinks = await _pageService.GetPageLinks();

            return PartialView(MVC.Home.Views._MenuBar, pageLinks);
        }

        [Route("Header")]
        public virtual async Task<ActionResult> Header()
        {
            var pageLinks = await _pageService.GetPageLinks();

            return PartialView(MVC.Home.Views._Header, pageLinks);
        }

        [Route("Footer")]
        public virtual async Task<ActionResult> Footer()
        {
            var links = new FooterViewModel
            {
                PageLinks = await _pageService.GetPageLinks(),
                PostCategoryLinks = await _postCategoryService.GetCategoryLinks()
            };

            return PartialView(MVC.Home.Views._Footer, links);
        }

    }
}