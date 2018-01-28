using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region CategoryDataGridViewModel
    public class CategoryDataGridViewModel : IHaveCustomMappings
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public int PostsCount { get; set; }
        public int Order { get; set; }
        #endregion

        #region CreateMappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PostCategory, CategoryDataGridViewModel>()
                .ForMember(catModel => catModel.PostsCount, opt => opt.MapFrom(postCategory => postCategory.Posts.Count));

            configuration.CreateMap<Category, CategoryDataGridViewModel>()
                .ForMember(catModel => catModel.PostsCount, opt => opt.MapFrom(postCategory => postCategory.Products.Count))
                .ForMember(catModel => catModel.Order, opt => opt.Ignore());
        }
        #endregion
    }
    #endregion
}
