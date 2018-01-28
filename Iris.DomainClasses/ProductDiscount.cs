using System;

namespace Iris.DomainClasses
{
    public class ProductDiscount : BaseEntity
    {
        #region Properties
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        #endregion
    }
}
