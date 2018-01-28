using System;

namespace Iris.DomainClasses
{
    public class SlideShowImage : BaseEntity
    {
        #region Constractors
        public SlideShowImage()
        {
            CreatedDate = DateTime.Now;;
        }
        #endregion

        #region Properties
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public int Order { get; set; }
        public DateTime CreatedDate { get; set; }
        #endregion
    }
}
