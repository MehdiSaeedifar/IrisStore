using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class SlideShowDataGridViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public int Order { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<SlideShowImage, SlideShowDataGridViewModel>();
        }
    }
}
