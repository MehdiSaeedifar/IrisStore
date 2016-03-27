using System;

namespace Iris.DomainClasses
{
    public class ProductPrice : BaseEntity
    {
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
