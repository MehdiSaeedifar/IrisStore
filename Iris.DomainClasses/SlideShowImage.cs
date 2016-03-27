using System;

namespace Iris.DomainClasses
{
    public class SlideShowImage : BaseEntity
    {
        public SlideShowImage()
        {
            CreatedDate = DateTime.Now;;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public int Order { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
