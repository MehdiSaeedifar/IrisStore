using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    #region ResetPasswordViewModel
    public class ResetPasswordViewModel
    {
        #region Properties
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "گذرواژه حداقل باید 6 کاراکتر باشد!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "گذرواژه ها یکسان نیستند!")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
        #endregion
    }
    #endregion
}