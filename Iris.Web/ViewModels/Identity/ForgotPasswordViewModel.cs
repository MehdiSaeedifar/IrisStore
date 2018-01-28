using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    #region ForgotPasswordViewModel
    public class ForgotPasswordViewModel
    {
        #region Properties
        [Required(ErrorMessage = "وارد کردن پست الکترونیکی ضروری است")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل معتبر وارد نمایید")]
        [Display(Name = "پست الکترونیکی")]
        public string Email { get; set; }
        #endregion
    }
    #endregion
}