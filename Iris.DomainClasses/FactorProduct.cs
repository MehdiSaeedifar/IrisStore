using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses
{
    public class FactorProduct
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        public int FactorId { get; set; }
        public Factor Factor { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
