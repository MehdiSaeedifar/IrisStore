using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class CategoryDataGridViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PostsCount { get; set; }
        public int Order { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PostCategory, CategoryDataGridViewModel>()
                .ForMember(catModel => catModel.PostsCount, opt => opt.MapFrom(postCategory => postCategory.Posts.Count));

            configuration.CreateMap<Category, CategoryDataGridViewModel>()
                .ForMember(catModel => catModel.PostsCount, opt => opt.MapFrom(postCategory => postCategory.Products.Count))
                .ForMember(catModel => catModel.Order, opt => opt.Ignore());
        }
    }
}
