using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    #region SetPasswordViewModel
    public class SetPasswordViewModel
    {
        #region Properties
        [Required]
        [StringLength(100, ErrorMessage = "گذرواژه باید حداقل 6 کاراکتر باشد!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "گذرواژه ها یکسان نیستند!")]
        public string ConfirmPassword { get; set; }
        #endregion
    }
    #endregion
}