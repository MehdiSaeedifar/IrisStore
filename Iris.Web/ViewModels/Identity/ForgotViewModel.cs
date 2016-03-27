using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}