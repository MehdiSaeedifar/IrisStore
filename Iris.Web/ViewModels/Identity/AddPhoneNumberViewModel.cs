using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    #region AddPhoneNumberViewModel
    public class AddPhoneNumberViewModel
    {
        #region Properties
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
        #endregion\
    }
    #endregion
}