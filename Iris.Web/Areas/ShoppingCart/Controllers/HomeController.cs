using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Iris.DataLayer;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using Microsoft.AspNet.Identity;

namespace Iris.Web.Areas.ShoppingCart.Controllers
{
    [RouteArea("ShoppingCart", AreaPrefix = "ShpppingCart")]
    public partial class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IProductService productService, IShoppingCartService shoppingCartService,
            IUnitOfWork unitOfWork)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _unitOfWork = unitOfWork;
        }

        [Route]
        public virtual ActionResult Index()
        {
            return View();
        }

        [Route("GetOrdersList")]
        public virtual async Task<ActionResult> GetOrdersList(int[] productIds)
        {
            return PartialView(await _productService.GetProductsOrders(productIds));
        }

        [Route("CreateFactor")]
        [HttpGet]
        public virtual ActionResult CreateFactor()
        {
            return View();
        }

        [Route("CreateFactor")]
        [HttpPost]
        public virtual async Task<ActionResult> CreateFactor(CreateFactorViewModel factorViewModel)
        {
            await _shoppingCartService.CreateFactor(factorViewModel);
            await _unitOfWork.SaveAllChangesAsync();
            return Content(Url.Action("UserFactor", "Home", new { area = "ShoppingCart" }));
        }


        [Route("UserFactor")]
        [HttpGet]
        public virtual async Task<ActionResult> UserFactor()
        {
            return View(await _shoppingCartService.GetUserFactor(Convert.ToInt32(User.Identity.GetUserId())));
        }

    }
}