using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.Web.Areas.SlideShow.Controllers
{
    #region AdminSlideShowController
    [Authorize(Roles = "Admin")]
    [RouteArea("SlideShow", AreaPrefix = "SlideShow-Admin")]
    public partial class AdminController : Controller
    {
        #region Feilds
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISlideShowImageService _slideShowService;
        private readonly IMappingEngine _mappingEngine;
        #endregion

        #region Constructors
        public AdminController(IUnitOfWork unitOfWork, ISlideShowImageService slideShowService,
            IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _slideShowService = slideShowService;
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

        #region GetSlideShows
        [Route("GetSlideShows")]
        public virtual async Task<ActionResult> GetSlideShows(JqGridRequest request)
        {
            var pageIndex = request.page - 1;
            var pageSize = request.rows;

            var list = await _slideShowService.GetDataGridSource(request.sidx + " " + request.sord,
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
                        product.Image,
                        product.Title,
                        product.Description,
                        product.Link,
                        product.Order
                    }
                })).ToList()
            };
            return Json(jqGridData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AddSlideShow
        [Route("Add")]
        public virtual async Task<ActionResult> AddSlideShow()
        {

            return View(new SlideShowViewModel
            {
                SlideShowImages = await _slideShowService.GetSlideShowImages()
            });
        }
        
        [Route("Add")]
        [HttpPost]
        public virtual async Task<ActionResult> AddSlideShow(SlideShowViewModel slideShowModel)
        {

            if (!ModelState.IsValid)
            {
                slideShowModel.SlideShowImages = await _slideShowService.GetSlideShowImages();

                return View(slideShowModel);
            }

            var slide = new SlideShowImage();

            _mappingEngine.Map(slideShowModel, slide);

            var otherSlideShows = new List<SlideShowImage>();

            _mappingEngine.Map<IList<SlideShowViewModel>, IList<SlideShowImage>>(slideShowModel.SlideShowImages,
                otherSlideShows);

            if (slideShowModel.Id.HasValue)
            {
                _slideShowService.EditSlide(slide, otherSlideShows);
                TempData["message"] = "اسلاید مورد نظر با موفقیت ویرایش شد";
            }
            else
            {
                _slideShowService.AddSlide(slide, otherSlideShows);
                TempData["message"] = "اسلاید جدید با موفقیت در سیستم ثبت شد";
            }


            await _unitOfWork.SaveAllChangesAsync();

            return RedirectToAction(MVC.SlideShow.Admin.ActionNames.Index);
        }
        #endregion

        #region EditSlideShow
        [Route("EditSlideShow/{id:int?}")]
        public virtual async Task<ActionResult> EditSlideShow(int id)
        {
            var selectedSlideShow = await _slideShowService.GetSlideShow(id);

            selectedSlideShow.SlideShowImages = await _slideShowService.GetSlideShowImages();

            return View((string)MVC.SlideShow.Admin.Views.AddSlideShow, selectedSlideShow);
        }
        #endregion

        #region DeleteSlideShow
        [Route("DeleteSlideShow")]
        public virtual async Task<ActionResult> DeleteSlideShow(int id)
        {
            _slideShowService.DeleteSlide(id);

            await _unitOfWork.SaveAllChangesAsync();

            return Json("ok");
        }
        #endregion
    }
    #endregion
}