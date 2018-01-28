using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    #region VerifyPhoneNumberViewModel
    public class VerifyPhoneNumberViewModel
    {
        #region Properties
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        #endregion
    }
    #endregion
}