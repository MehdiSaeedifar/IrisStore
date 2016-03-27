using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Iris.DataLayer;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;
using Microsoft.AspNet.Identity;

namespace Iris.Web.Areas.Page.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("Page", AreaPrefix = "Page-Admin")]
    public partial class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPageService _pageService;
        private readonly IMappingEngine _mappingEngine;

        public AdminController(IUnitOfWork unitOfWork, IPageService pageService, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _pageService = pageService;
            _mappingEngine = mappingEngine;
        }

        [Route("List")]
        public virtual ActionResult Index()
        {
            return View();
        }


        [Route("GetDataGridData")]
        public virtual async Task<ActionResult> GetDataGridData(JqGridRequest request)
        {
            var pageIndex = request.page - 1;
            var pageSize = request.rows;

            var list = await _pageService.GetDataGridSource(request.sidx + " " + request.sord,
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
                        product.Title,
                        product.ViewNumber,
                        product.Order
                    }
                })).ToList()
            };
            return Json(jqGridData, JsonRequestBehavior.AllowGet);
        }


        [Route("Add")]
        public virtual async Task<ActionResult> Add()
        {
            var model = new AddPageViewModel { Pages = await _pageService.GetAllPagesForAdd() };

            return View(model);
        }

        [Route("Add")]
        [HttpPost]
        public virtual async Task<ActionResult> Add(AddPageViewModel pageModel)
        {

            if (!ModelState.IsValid)
            {
                pageModel.Pages = await _pageService.GetAllPagesForAdd();

                return View(pageModel);
            }

            var page = new DomainClasses.Page()
            {
                PostedByUserId = User.Identity.GetUserId<int>()
            };

            _mappingEngine.Map(pageModel, page);

            if (pageModel.Id.HasValue)
            {
                _pageService.EditPage(page, pageModel.Pages.Select(x => new DomainClasses.Page
                {
                    Id = x.Id ?? 0,
                    Order = x.Order
                }).ToList());

                TempData["message"] = "برگه مورد نظر با موفقیت ویرایش شد";
            }
            else
            {
                _pageService.AddPage(page, pageModel.Pages.Select(x => new DomainClasses.Page
                {
                    Id = x.Id ?? 0,
                    Order = x.Order
                }).ToList());

                TempData["message"] = "برگه جدید با موفقیت در سیستم ثبت شد";
            }

            await _unitOfWork.SaveAllChangesAsync();

            return RedirectToAction(MVC.Page.Admin.ActionNames.Index);
        }

        [Route("Edit/{id:int?}")]
        public virtual async Task<ActionResult> Edit(int id)
        {
            var model = await _pageService.GetPage(id);

            model.Pages = await _pageService.GetAllPagesForAdd();

            return View(MVC.Page.Admin.Views.Add, model);
        }

        [Route("Delete")]
        [HttpPost]
        public virtual async Task<ActionResult> Delete(int id)
        {
            _pageService.DeletePage(id);

            await _unitOfWork.SaveAllChangesAsync();

            return Json("ok");
        }

    }
}