using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Iris.DataLayer;
using Iris.LuceneSearch;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;
using Microsoft.AspNet.Identity;
using Utilities;

namespace Iris.Web.Areas.Post.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("Post", AreaPrefix = "Post-Admin")]
    public partial class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostService _postService;
        private readonly IMappingEngine _mappingEngine;
        private readonly IPostCategoryService _postCategoryService;

        public AdminController(IUnitOfWork unitOfWork, IPostService pageService, IMappingEngine mappingEngine, IPostCategoryService postCategoryService)
        {
            _unitOfWork = unitOfWork;
            _postService = pageService;
            _mappingEngine = mappingEngine;
            _postCategoryService = postCategoryService;
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

            var list = await _postService.GetDataGridSource(request.sidx + " " + request.sord,
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
                        product.CategoryName,
                        product.ViewNumber,
                    }
                })).ToList()
            };
            return Json(jqGridData, JsonRequestBehavior.AllowGet);
        }


        [Route("Add")]
        public virtual async Task<ActionResult> Add()
        {
            ViewData["CategoriesSelectList"] = new SelectList(await _postCategoryService.GetAll(), "Id", "Name");

            var model = new AddPostViewModel();

            return View(model);
        }

        [Route("Add")]
        [HttpPost]
        public virtual async Task<ActionResult> Add(AddPostViewModel postModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoriesSelectList"] = new SelectList(await _postCategoryService.GetAll(), "Id", "Name");

                return View(postModel);
            }

            var post = new DomainClasses.Post
            {
                PostedByUserId = User.Identity.GetUserId<int>()
            };

            _mappingEngine.Map(postModel, post);

            if (postModel.Id.HasValue)
            {
                _postService.Edit(post);
                TempData["message"] = "پست مورد نظر با موفقیت ویرایش شد";
            }
            else
            {
                _postService.Add(post);
                TempData["message"] = "پست جدید با موفقیت در سیستم ثبت شد";
            }

            await _unitOfWork.SaveAllChangesAsync();


            if (postModel.Id.HasValue)
            {
                LuceneIndex.ClearLucenePostIndexRecord(postModel.Id.Value);
            }

            LuceneIndex.AddUpdateLuceneIndex(new LuceneSearchModel
            {
                PostId = post.Id,
                Title = post.Title,
                Image = post.Image,
                Description = post.Body.RemoveHtmlTags(),
                Category = await _postCategoryService.GetCategoryName(postModel.CategoryId.Value),
                SlugUrl = post.SlugUrl
            });


            return RedirectToAction(MVC.Post.Admin.ActionNames.Index);
        }

        [Route("Edit/{id:int?}")]
        public virtual async Task<ActionResult> Edit(int id)
        {
            var model = await _postService.GetPostForEdit(id);

            ViewData["CategoriesSelectList"] = new SelectList(await _postCategoryService.GetAll(), "Id", "Name");

            return View(MVC.Post.Admin.Views.Add, model);
        }

        [Route("Delete")]
        [HttpPost]
        public virtual async Task<ActionResult> Delete(int id)
        {
            _postService.Delete(id);

            await _unitOfWork.SaveAllChangesAsync();

            LuceneIndex.ClearLucenePostIndexRecord(id);

            return Json("ok");
        }

    }
}