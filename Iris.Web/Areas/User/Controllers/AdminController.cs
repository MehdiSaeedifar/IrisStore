using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using AutoMapper;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.Web.ViewModels.Identity;
using JqGridHelper.DynamicSearch;
using JqGridHelper.Models;

namespace Iris.Web.Areas.User.Controllers
{
    #region AdminUserController
    [Authorize(Roles = "Admin")]
    [RouteArea("User", AreaPrefix = "User-Admin")]
    public partial class AdminController : Controller
    {
        #region Feilds
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IApplicationUserManager _userManager;
        #endregion

        #region Constructors
        public AdminController(IUnitOfWork unitOfWork, IMappingEngine mappingEngine, IApplicationUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _userManager = userManager;
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

            var list = await _userManager.GetDataGridSource(request.sidx + " " + request.sord,
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
                        product.UserName,
                        product.Email,
                    }
                })).ToList()
            };
            return Json(jqGridData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Add
        [Route("Add")]
        public virtual ActionResult Add()
        {
            return View(new RegisterViewModel());
        }

        [Route("Add")]
        [HttpPost]
        public virtual async Task<ActionResult> Add(RegisterViewModel userViewModel, HttpPostedFileBase userImage)
        {
            if (userViewModel.Id.HasValue)
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            if (!userViewModel.Id.HasValue)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.UserName,
                    Email = userViewModel.Email,
                    EmailConfirmed = true
                };

                var adminresult = await _userManager.CreateAsync(user, userViewModel.Password);

                if (adminresult.Succeeded)
                {
                    var result = await _userManager.AddToRolesAsync(user.Id, "Admin");
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First());
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());

                    return View();
                }

                TempData["message"] = "کاربر جدید با موفقیت در سیستم ثبت شد";

            }
            else
            {
                var user = await _userManager.FindByIdAsync(userViewModel.Id.Value);

                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;

                await _unitOfWork.SaveAllChangesAsync();

                TempData["message"] = "کاربر مورد نظر با موفقیت ویرایش شد";

            }

            if (userImage != null)
            {
                var img = new WebImage(userImage.InputStream);
                img.Resize(161, 161, true, false).Crop(1, 1);

                img.Save(Server.MapPath("~/UploadedFiles/Avatars/" + userViewModel.UserName + ".png"));
            }


            return RedirectToAction(MVC.User.Admin.ActionNames.Index);
        }
        #endregion

        #region Edit
        [Route("Edit/{id:int?}")]
        public virtual async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await _userManager.FindByIdAsync(id.Value);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View((string) MVC.User.Admin.Views.Add, new RegisterViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            });
        }
        #endregion

        #region Delete
        [Route("Delete")]
        [HttpPost]
        public virtual async Task<ActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Errors.First());
                return View();
            }

            return Json("ok");
        }
        #endregion
    }
    #endregion
}