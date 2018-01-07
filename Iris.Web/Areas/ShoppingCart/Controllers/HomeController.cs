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
using Iris.Web.ViewModels.Identity;

namespace Iris.Web.Areas.ShoppingCart.Controllers
{
    [RouteArea("ShoppingCart", AreaPrefix = "ShpppingCart")]
    public partial class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserManager _userManager;


        public HomeController(IProductService productService, IApplicationUserManager userManager, IShoppingCartService shoppingCartService,
            IUnitOfWork unitOfWork)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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
        public virtual async Task<ActionResult> CreateFactor()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.Mobile = user.Mobile;
                ViewBag.Address = user.Address;
            }
            return View();
        }

        [Route("CreateFactor")]
        [HttpPost]
        public virtual async Task<ActionResult> CreateFactor(CreateFactorViewModel factorViewModel)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return View();
            }
            else
            {
                if (string.IsNullOrEmpty(user.FirstName) && string.IsNullOrEmpty(user.LastName) && string.IsNullOrEmpty(user.Mobile) && string.IsNullOrEmpty(user.Address))
                {
                    user.FirstName = factorViewModel.Name;
                    user.LastName = factorViewModel.LastName;
                    user.Mobile = factorViewModel.PhoneNumber;
                    user.Address = factorViewModel.Address;
                }

                await _shoppingCartService.CreateFactor(factorViewModel);
                await _unitOfWork.SaveAllChangesAsync();
                return Content(Url.Action("UserFactor", "Home", new { area = "ShoppingCart" }));
            }
        }


        [Route("UserFactor")]
        [HttpGet]
        public virtual async Task<ActionResult> UserFactor()
        {
            return View(await _shoppingCartService.GetUserFactor(Convert.ToInt32(User.Identity.GetUserId())));
        }

    }
}