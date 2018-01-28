using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    #region ChangePasswordViewModel
    public class ChangePasswordViewModel
    {
        #region Properties
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور فعلی")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = " 'گذرواژه حداقل باید 6 کاراکتر باشد!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور جدید")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور")]
        [Compare("NewPassword", ErrorMessage = "گذرواژها یکسان نیستند!.")]
        public string ConfirmPassword { get; set; }
        #endregion
    }
    #endregion
}