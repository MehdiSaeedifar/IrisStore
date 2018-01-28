using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    #region ExternalLoginConfirmationViewModel
    public class ExternalLoginConfirmationViewModel
    {
        #region Properties
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        #endregion
    }
    #endregion
}