using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Iris.DataLayer;
using Iris.LuceneSearch;
using Iris.ServiceLayer.Contracts;
using Iris.Web.Helpers;

namespace Iris.Web.Areas.Product.Controllers
{
    #region HomeProductController
    [RouteArea("Product", AreaPrefix = "product")]
    public partial class HomeController : Controller
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        #endregion

        #region Constructors
        public HomeController(IUnitOfWork unitOfWork, IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
        }
        #endregion

        #region Index
        [Route("{id:int?}/{slugUrl?}")]
        public virtual async Task<ActionResult> Index(int id)
        {
            var model = await _productService.GetProductPage(id);

            ViewData["SimilarProducts"] = LuceneIndex.GetMoreLikeThisProjectItems(id)
                    .Where(item => item.Category == "کالا‌ها").Skip(1).Take(8).ToList();

            await _unitOfWork.SaveAllChangesAsync(false);

            var keywords = "";

            if (model.Categories != null && model.Categories.Any())
            {
                keywords = model.Categories.Aggregate(keywords, (current, category) => current + (category.Name + "-"));
            }

            keywords = keywords.Substring(0, keywords.Length - 1);

            ViewBag.Keywords = keywords;

            ViewBag.MetaDescription = model.MetaDescription;

            return View(model);
        }
        #endregion

        #region SaveRatings
        [Route("SaveRatings")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> SaveRatings(int id, double value)
        {
            var sectionType = "Product";

            if (!this.HttpContext.CanUserVoteBasedOnCookies(id, sectionType))
                return Content("nok"); //اعلام فقط یکبار مجاز هستید رای دهید

            await _productService.SaveRating(id, value);

            await _unitOfWork.SaveAllChangesAsync(false);

            return Content("ok"); //اعلام موفقیت آمیز بودن ثبت اطلاعات
        }
        #endregion
    }
    #endregion
}