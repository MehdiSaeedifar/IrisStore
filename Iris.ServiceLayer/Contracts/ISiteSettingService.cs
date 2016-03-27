using System.Threading.Tasks;
using Iris.ViewModels;

namespace Iris.ServiceLayer.Contracts
{
    public interface ISiteSettingService
    {
        Task UpdateSettings(EditSettingViewModel settingsModel);
        Task<EditSettingViewModel> GetSettingsForEdit();
        Task<string> GetSiteName();
        Task<string> GetContactInfo();
        Task<SiteMetaTags> GetSiteMetaTags();
        Task<string> GetNotificationEmail();
        Task<SmtpSettings> GetSmtpSettings();
        EditSettingViewModel GetSettings();
    }

    public class SiteFooterInfo
    {
        public string ContactInfo { get; set; }
    }

    public class SiteMetaTags
    {
        public string Keywords { get; set; }
        public string Description { get; set; }
    }


}
