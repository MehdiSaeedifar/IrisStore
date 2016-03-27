using Iris.ViewModels;

namespace Iris.Web.Caching
{
    public interface ICacheService
    {
        EditSettingViewModel GetSiteSettings();
        void RemoveSiteSettings();
    }
}