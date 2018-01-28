using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Iris.DataLayer;
using Iris.ServiceLayer;
using Iris.ServiceLayer.Contracts;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.Web.Areas.PostCategory.Controllers
{
    #region AdminPostCategoryController
    [Authorize(Roles = "Admin")]
    [RouteArea("PostCategory", AreaPrefix = "PostCategory-Admin")]
    public partial class AdminController : Controller
    {
        #region Feild
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostCategoryService _categoryService;
        private readonly IMappingEngine _mappingEngine;
        #endregion

        #region Constructors
        public AdminController(IUnitOfWork unitOfWork, IPostCategoryService categoryService, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
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

        #region GetDataGridData
        [Route("GetDataGridData")]
        public virtual async Task<ActionResult> GetDataGridData(JqGridRequest request)
        {
            var pageIndex = request.page - 1;
            var pageSize = request.rows;

            var list = await _categoryService.GetDataGridSource(request.sidx + " " + request.sord,
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
                        product.PostsCount,
                        product.Order
                    }
                })).ToList()
            };
            return Json(jqGridData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add
        [Route("Add")]
        [HttpPost]
        public virtual async Task<ActionResult> Add(AddPostCategoryViewModel categoryModel)
        {
            var postCategory = new Iris.DomainClasses.PostCategory();
            {
            };

            _mappingEngine.Map(categoryModel, postCategory);

            if (categoryModel.Id.HasValue)
            {
                _categoryService.Edit(postCategory);
            }
            else
            {
                _categoryService.Add(postCategory);
            }

            await _unitOfWork.SaveAllChangesAsync();

            return Json(new { id = postCategory.Id, success = true });
        }
        #endregion

        #region Delete
        [Route("Delete")]
        [HttpPost]
        public virtual async Task<ActionResult> Delete(int id)
        {
            _categoryService.Delete(id);
            await _unitOfWork.SaveAllChangesAsync();

            return Json("ok");
        }
        #endregion
    }
    #endregion
}