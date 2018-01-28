using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Iris.Web.ViewModels.Identity
{
    #region EditUserViewModel
    public class EditUserViewModel
    {
        #region Properties
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
        #endregion
    }
    #endregion
}