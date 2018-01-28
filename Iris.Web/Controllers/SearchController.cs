using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Iris.LuceneSearch;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using Iris.Web.ViewModels;
using Utilities;

namespace Iris.Web.Controllers
{
    #region SearchController
    [RoutePrefix("Search")]
    public partial class SearchController : Controller
    {
        #region Feilds
        private readonly ICategoryService _categoryService;
        #endregion

        #region Constractors
        public SearchController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        #endregion

        #region AutoCompleteSearch
        [Route("AutoCompleteSearch")]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> AutoCompleteSearch(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Content(string.Empty);

            var categories = await _categoryService.SearchCategory(term, 5);

            var items =
                LuceneIndex.Search(term, "Title", "Description").Take(10).ToList();

            var luceneList = items.Where(x => x.Category == "کالا‌ها").Select(item => new AutoCompleteSearchViewModel
            {
                Label = item.Title,
                Url = Url.Action(MVC.Product.Home.ActionNames.Index, MVC.Product.Home.Name, new { area = MVC.Product.Name, id = item.ProductId, slugUrl = item.SlugUrl }),
                Category = item.Category,
                Image = item.Image,
            }).ToList();

            luceneList.AddRange(items.Where(x => x.Category != "کالا‌ها").Select(item => new AutoCompleteSearchViewModel
            {
                Label = item.Title,
                Url = Url.Action(MVC.Post.Home.ActionNames.Index, MVC.Post.Home.Name, new { area = MVC.Post.Name, id = item.PostId, slugUrl = item.SlugUrl }),
                Category = item.Category,
                Image = item.Image
            }));


            var data = categories.Select(x => new AutoCompleteSearchViewModel
            {
                Label = x.Name,
                Url = $"{Url.Action(MVC.Product.SearchProduct.ActionNames.Index, MVC.Product.SearchProduct.Name, new { area = MVC.Product.Name })}#/page/{x.Id}",
                Category = "گروه‌ها"
            }).Concat(luceneList);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
    #endregion
}