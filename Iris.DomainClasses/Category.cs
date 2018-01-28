using System.Collections.Generic;

namespace Iris.DomainClasses
{
    public class Category
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        #endregion
    }
}
