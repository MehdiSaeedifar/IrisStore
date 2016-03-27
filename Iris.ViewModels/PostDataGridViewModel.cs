using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class PostDataGridViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ViewNumber { get; set; }
        public string CategoryName { get; set; }

        public virtual void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, PostDataGridViewModel>()
                .ForMember(postModel => postModel.CategoryName, opt => opt.MapFrom(post => post.Category.Name));
        }
    }
}
