using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using PagedList;

namespace Iris.Web.Areas.Product.Controllers
{
    #region SearchProductController
    [RouteArea("Product", AreaPrefix = "product")]
    public partial class SearchProductController : Controller
    {
        #region Feilds
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IMappingEngine _mappingEngine;
        #endregion

        #region Constructors
        public SearchProductController(ICategoryService categoryService, IMappingEngine mappingEngine, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _mappingEngine = mappingEngine;
        }
        #endregion

        #region SearchIndex
        [Route("Search")]
        public virtual async Task<ActionResult> Index()
        {
            var model = new SerachProductIndexViewModel
            {
                Categories = new GroupsViewModel
                {
                    SelectedGroups = new List<GroupViewModel>(),
                    AvailableGroups = _mappingEngine.Map<IList<CategoryViewModel>, IList<GroupViewModel>>((await _categoryService.GetSearchProductsCategories())),
                },
                Prices = await _productService.GetAvailableProductPrices(),
                Discounts = await _productService.GetAvailableProductPrices()

            };

            return View(model);
        }
        #endregion

        #region GetProducts
        [Route("GetProducts")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> GetProducts(SearchProductViewModel model)
        {
            var result = await _productService.SearchProduct(model);

            var productsAsIPagedList = new StaticPagedList<ProductWidgetViewModel>(result.Products, model.PageNumber, model.PageSize, result.TotalCount);

            return PartialView(MVC.Product.SearchProduct.Views._GetProducts,
                               productsAsIPagedList);
        }
        #endregion
    }
    #endregion
}