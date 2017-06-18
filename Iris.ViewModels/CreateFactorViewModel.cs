using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class CreateFactorViewModel
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "لطفا نام را وارد کنید")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا نام خانوادگی را وارد کنید")]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا شماره تلفن را وارد کنید")]
        public string PhoneNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا آدرس را وارد کنید")]
        public string Address { get; set; }

        public IList<FactorPorductViewModel> Products { get; set; }
    }

    public class FactorPorductViewModel
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
