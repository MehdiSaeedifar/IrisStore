using System.ComponentModel.DataAnnotations;

namespace Iris.Web.ViewModels.Identity
{
    #region RoleViewModel
    public class RoleViewModel
    {
        #region Properties
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
        #endregion
    }
    #endregion
}