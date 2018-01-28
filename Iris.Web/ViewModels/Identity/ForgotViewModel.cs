using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    #region ForgotViewModel
    public class ForgotViewModel
    {
        #region Properties
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        #endregion
    }
    #endregion
}