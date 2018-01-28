using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Iris.ViewModels
{
    #region EditSettingViewModel
    public class EditSettingViewModel
    {
        #region Properties
        [DisplayName("آدرس سایت")]
        public string SiteUrl { get; set; }

        [DisplayName("نام سایت")]
        public string SiteName { get; set; }

        [DisplayName("کلمات کلیدی")]
        [DataType(DataType.MultilineText)]
        public string SiteKeywords { get; set; }

        [DisplayName("توضیحات")]
        [DataType(DataType.MultilineText)]
        public string SiteDescription { get; set; }

        [Display(Name = "سرویس دهنده ایمیل")]
        public string MailServerUrl { get; set; }

        [Display(Name = "درگاه")]
        public string MailServerPort { get; set; }

        [Display(Name = "شناسه ورود")]
        public string MailServerUserName { get; set; }

        [Display(Name = "کلمه عبور")]
        public string MailServerPassword { get; set; }

        [DisplayName("اطلاعات تماس")]
        [DataType(DataType.MultilineText)]
        public string ContactInfo { get; set; }
        #endregion
    }
    #endregion
}
