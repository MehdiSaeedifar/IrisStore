using System.Web;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using Utilities;

namespace Iris.Web.Caching
{
    public class CacheService : ICacheService
    {
        public const string SiteSettingsKey = "SiteSettings";

        private readonly HttpContextBase _httpContext;
        private readonly ISiteSettingService _settingService;

        public CacheService(HttpContextBase httpContext, ISiteSettingService settingService)
        {
            _httpContext = httpContext;
            _settingService = settingService;
        }

        public EditSettingViewModel GetSiteSettings()
        {
            var siteSettings = _httpContext.CacheRead<EditSettingViewModel>(SiteSettingsKey);

            const int durationMinutes = 60;


            if (siteSettings != null) return siteSettings;

            siteSettings = _settingService.GetSettings();
            _httpContext.CacheInsert(SiteSettingsKey, siteSettings, durationMinutes);

            return siteSettings;
        }

        public void RemoveSiteSettings()
        {
            _httpContext.InvalidateCache(SiteSettingsKey);
        }

    }
}