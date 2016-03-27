using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Iris.LuceneSearch;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using Iris.Web.DependencyResolution;
using Utilities;

namespace Iris.Web.Controllers
{
    [RoutePrefix("LuceneIndexing")]
    [Authorize(Roles = "Admin")]
    public class LuceneIndexingController : Controller
    {
        [Route("ReIndex")]
        public async Task<ActionResult> ReIndex()
        {
            LuceneIndex.ClearLuceneIndex();

            var productService = IoC.Container.GetInstance<IProductService>();
            var postService = IoC.Container.GetInstance<IPostService>();

            foreach (var product in await productService.GetAllForLuceneIndex())
            {
                LuceneIndex.ClearLuceneIndexRecord(product.Id);
                LuceneIndex.AddUpdateLuceneIndex(new LuceneSearchModel
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    Image = product.Image,
                    Description = product.Description.RemoveHtmlTags(),
                    Category = "کالا‌ها",
                    SlugUrl = product.SlugUrl,
                    Price = product.Price.ToString(CultureInfo.InvariantCulture),
                    ProductStatus = product.ProductStatus.ToString()
                });
            }

            foreach (var post in await postService.GetAllForLuceneIndex())
            {
                LuceneIndex.ClearLucenePostIndexRecord(post.Id);
                LuceneIndex.AddUpdateLuceneIndex(new LuceneSearchModel
                {
                    PostId = post.Id,
                    Title = post.Title,
                    Image = post.Image,
                    Description = post.Description.RemoveHtmlTags(),
                    Category = post.Category,
                    SlugUrl = post.SlugUrl
                });
            }
            return Content("ReIndexing Complete.");
        }
    }
}