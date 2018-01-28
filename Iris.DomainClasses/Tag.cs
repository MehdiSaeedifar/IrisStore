using System.Collections.Generic;

namespace Iris.DomainClasses
{
    public class Tag
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        #endregion
    }
}
