using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    #region LoginViewModel
    public class LoginViewModel
    {
        #region Properties
        [Required(AllowEmptyStrings = false, ErrorMessage = "وارد کردن پست الکترونیکی ضروری است")]
        [Display(Name = "پست الکترونیکی")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل معتبر وارد نمایید")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "وارد کردن کلمه عبور ضروری است")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به یاد داشته باش؟")]
        public bool RememberMe { get; set; }
        #endregion
    }
    #endregion
}