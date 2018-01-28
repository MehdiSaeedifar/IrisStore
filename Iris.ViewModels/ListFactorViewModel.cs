using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region ListFactorViewModel
    public class ListFactorViewModel 
    {
        #region Properties
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا نام را وارد کنید")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا نام خانوادگی را وارد کنید")]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا شماره تلفن را وارد کنید")]
        public string PhoneNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا آدرس را وارد کنید")]
        public string Address { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;
        [Required]
        public FactorStatus Status { get; set; } = FactorStatus.Paid;
        public IList<ListFactorProductViewModel> Products { get; set; }
        #endregion
    }
    #endregion

    #region ListFactorProductViewModel
    public class ListFactorProductViewModel 
    {
        #region Properties
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Count { get; set; }
        public int MaxCount { get; set; } = 0;
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        #region Calculator Properties
        /// <summary>
        /// Calculator Discounts
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CalcDiscount { get { return (Price - ((Price * Discount) / 100)); } }
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CalcDiscountFee { get { return (((Price * Discount) / 100)); } }
        #endregion

        #endregion
    }
    #endregion
}
