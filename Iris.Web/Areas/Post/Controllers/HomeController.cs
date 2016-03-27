using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Iris.DataLayer;
using Iris.LuceneSearch;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using PagedList;

namespace Iris.Web.Areas.Post.Controllers
{
    [RouteArea("Post", AreaPrefix = "Post")]
    public partial class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostService _postService;
        private readonly IPostCategoryService _postCategoryService;

        public HomeController(IUnitOfWork unitOfWork, IPostService postService, IPostCategoryService postCategoryService)
        {
            _unitOfWork = unitOfWork;
            _postService = postService;
            _postCategoryService = postCategoryService;
        }

        [Route("{id:int}/{slugUrl?}")]
        public virtual async Task<ActionResult> Index(int id)
        {
            var post = await _postService.GetPost(id);

            await _unitOfWork.SaveAllChangesAsync(false);

            ViewBag.Keywords = post.CategoryName;

            ViewBag.MetaDescription = post.MetaDescription;

            ViewBag.Author = post.AuthorName;

            ViewBag.LastModified = post.PostedDate.ToUniversalTime().ToString("ddd MMM dd yyyy HH:mm:ss \"GMT\"K");

            if (post.CategoryId.HasValue)
            {
                ViewData["SimilarProducts"] = LuceneIndex.GetMoreLikeThisPostItems(id)
                                                .Where(item => item.Category != "کالا‌ها").Skip(1).Take(8).ToList();
            }

            return View(post);
        }

        [Route("List/{id:int}/{slugUrl}/{page:int?}")]
        public virtual async Task<ActionResult> List(int id, int? page)
        {
            var pageIndex = page ?? 1 - 1;
            const int pageSize = 10;

            var postList = await _postService.GetPagedList(id, pageIndex, pageSize);

            ViewBag.OnePageOfList = new StaticPagedList<PagedListPostViewModel>(postList.List, pageIndex + 1, pageSize, postList.TotalCount);

            ViewBag.CategoryName = await _postCategoryService.GetCategoryName(id);

            return View();
        }

    }
}