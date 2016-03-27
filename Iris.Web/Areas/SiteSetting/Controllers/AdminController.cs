using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Iris.DataLayer;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using Iris.Web.Caching;
using Utilities;

namespace Iris.Web.Areas.SiteSetting.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("SiteSetting", AreaPrefix = "SiteSetting-Admin")]
    public partial class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISiteSettingService _settingService;
        private readonly ICacheService _cacheService;

        public AdminController(IUnitOfWork unitOfWork, ISiteSettingService settingService, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _settingService = settingService;
            _cacheService = cacheService;
        }

        [Route("Edit")]
        public virtual async Task<ActionResult> Edit()
        {
            var model = await _settingService.GetSettingsForEdit();

            if (!string.IsNullOrWhiteSpace(model.ContactInfo))
            {
                model.ContactInfo = model.ContactInfo.Replace("<br/>", Environment.NewLine);
            }

            return View(model);
        }

        [Route("Edit")]
        [HttpPost]
        public virtual async Task<ActionResult> Edit(EditSettingViewModel settingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(settingModel);
            }

            if (!string.IsNullOrWhiteSpace(settingModel.SiteDescription))
            {
                settingModel.SiteDescription = SeoHelpers.GenerateMetaDescription(settingModel.SiteDescription);
            }

            await _settingService.UpdateSettings(settingModel);

            await _unitOfWork.SaveAllChangesAsync();

            _cacheService.RemoveSiteSettings();

            TempData["message"] = "تنظیمات سایت با موفقیت ویرایش شد";

            return RedirectToAction(MVC.SiteSetting.Admin.ActionNames.Edit);
        }

    }
}