using System;

namespace Iris.DomainClasses
{
    public class ProductPrice : BaseEntity
    {
        #region Properties
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        #endregion
    }
}
