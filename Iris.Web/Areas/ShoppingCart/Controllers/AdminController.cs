using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.LuceneSearch;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;
using Utilities;

namespace Iris.Web.Areas.ShoppingCart.Controllers
{
    #region AdminShoppingCartController
    [Authorize(Roles = "Admin")]
    [RouteArea("ShoppingCart", AreaPrefix = "ShoppingCart-Admin")]
    public partial class AdminController : Controller
    {
        #region Feild
        private readonly IMappingEngine _mappingEngine;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICategoryService _categoryService;
        #endregion

        #region Constructors
        public AdminController(IUnitOfWork unitOfWork, IShoppingCartService shoppingCartService, ICategoryService categoryService,
            IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _shoppingCartService = shoppingCartService;
            _categoryService = categoryService;
            _mappingEngine = mappingEngine;
        }
        #endregion

        #region Index
        [Route("List")]
        public virtual ActionResult Index()
        {
            return View();
        }
        #endregion

        #region EditFactor
        [Route("EditFactor")]
        [HttpPost]
        public virtual async Task<ActionResult> EditFactor(ListFactorViewModel factorViewModel)
        {
            await _shoppingCartService.Edit(factorViewModel);
            await _unitOfWork.SaveAllChangesAsync();
            return Content(Url.Action("UserFactor", "Home", new { area = "ShoppingCart" }));
        }
        #endregion

        #region GetFactors
        [Route("GetFactors")]
        public virtual async Task<ActionResult> GetFactors(JqGridRequest request, string hiddenColumns)
        {
            var pageIndex = request.page - 1;
            var pageSize = request.rows;

            var list = await _shoppingCartService.GetDataGridSource(request.sidx + " " + request.sord,
                        request, Request.Form, DateTimeType.Persian, pageIndex, pageSize);


            var totalRecords = list.TotalCount;
            var totalPages = (int)Math.Ceiling(totalRecords / (float)pageSize);


            var jqGridData = new JqGridData
            {
                UserData = new // نمایش در فوتر
                {
                    Name = "جمع صفحه",
                    Price = 22
                },
                Total = totalPages,
                Page = request.page,
                Records = totalRecords,
                Rows = (list.Records.Select(product => new JqGridRowData
                {
                    Id = product.Id,
                    RowCells = new List<object>
                                                    {
                                                        product.Id.ToString(CultureInfo.InvariantCulture),
                                                        product.Name,
                                                        product.LastName,
                                                        product.IssueDate.ToPersianDate(),
                                                        product.Status.GetEnumDescription()
                                                    }
                })).ToList()
            };
            return Json(jqGridData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AddProduct
        [Route("Add")]
        public virtual async Task<ActionResult> AddProduct()
        {
            var productModel = new AddProductViewModel();

            ViewData["CategoriesSelectList"] = new MultiSelectList(await _categoryService.GetAll(), "Name", "Name");

            return View(productModel);
        }
        #endregion

        #region Edit
        [Route("Edit/{id:int?}")]
        [HttpGet]
        public virtual async Task<ActionResult> Edit(int id)
        {
            var selectedProduct = await _shoppingCartService.GetForEdit(id);


            return View(selectedProduct);
        }
         
        [Route("Edit")]
        [HttpPost]
        public virtual async Task<ActionResult> Edit(ListFactorViewModel factorViewModel)
        {
            await _shoppingCartService.Edit(factorViewModel);
             await _unitOfWork.SaveAllChangesAsync();
            TempData["message"] = "فاکتور مورد نظر با موفقیت ویرایش شد.";
            return Json(true);
        }
        #endregion

        #region Delete
        [Route("Delete")]
        [HttpPost]
        public virtual async Task<ActionResult> Delete(int id)
        {
            _shoppingCartService.Delete(id);
            await _unitOfWork.SaveAllChangesAsync();

            return Json(true);
        }
        #endregion
    }
    #endregion
}