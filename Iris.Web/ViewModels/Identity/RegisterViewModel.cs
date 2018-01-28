using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    #region RegisterViewModel
    public class RegisterViewModel
    {
        #region Properties
        public int? Id { get; set; }

        [Required]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "پست الکترونیکی")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "کلمه عبور باید حداقل 6 حرف باشد", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور")]
        [Compare("Password", ErrorMessage = "تکرار کلمه عبور، با کلمه عبور یکسان نیست")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
        #endregion
    }
    #endregion
}