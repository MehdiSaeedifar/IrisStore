using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iris.ViewModels
{
    #region CreateFactorViewModel
    public class CreateFactorViewModel
    {
        #region Properties
        [Required(AllowEmptyStrings = false,ErrorMessage = "لطفا نام را وارد کنید")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا نام خانوادگی را وارد کنید")]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا شماره تلفن را وارد کنید")]
        public string PhoneNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا آدرس را وارد کنید")]
        public string Address { get; set; }

        public IList<FactorPorductViewModel> Products { get; set; }
        #endregion
    }
    #endregion

    #region FactorPorductViewModel
    public class FactorPorductViewModel
    {
        #region Properties
        public int ProductId { get; set; }
        public int Count { get; set; }
        #endregion
    }
    #endregion
}
