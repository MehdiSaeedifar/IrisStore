using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ServiceLayer
{
    public class AddPostCategoryViewModel : IHaveCustomMappings
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<AddPostCategoryViewModel, PostCategory>();

            configuration.CreateMap<Category, AddPostCategoryViewModel>()
                .ForMember(x => x.Order, opt => opt.Ignore()).ReverseMap();
        }
    }
}
