using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses
{
    public class FactorProduct
    {
        #region Properties
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Count { get; set; }

        public int FactorId { get; set; }
        public Factor Factor { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public decimal TotalPrice { get { return (Price - ((Price * Discount) / 100) * Count); } set {; } }
        public decimal TotalDiscount { get { return (((Price * Discount) / 100) * Count); } set {; } }
        #endregion
    }
}
