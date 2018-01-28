using System;
using System.Collections.Generic;

namespace Iris.DomainClasses
{
    public class Post : BaseEntity
    {
        #region Constractors
        public Post()
        {
            PostedDate = DateTime.Now;
        }
        #endregion

        #region Properties
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public int PostedByUserId { get; set; }
        public ApplicationUser PostedByUser { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public string MetaDescription { get; set; }
        public string SlugUrl { get; set; }
        public int ViewNumber { get; set; }
        public string Image { get; set; }
        public int? CategoryId { get; set; }
        public virtual PostCategory Category { get; set; }
        #endregion
    }
}
