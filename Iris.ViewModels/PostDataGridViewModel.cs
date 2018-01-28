using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region PostDataGridViewModel
    public class PostDataGridViewModel : IHaveCustomMappings
    {
        #region Properties
        public int Id { get; set; }
        public string Title { get; set; }
        public int ViewNumber { get; set; }
        public string CategoryName { get; set; }
        #endregion

        #region CreateMappings
        public virtual void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, PostDataGridViewModel>()
                .ForMember(postModel => postModel.CategoryName, opt => opt.MapFrom(post => post.Category.Name));
        }
        #endregion
    }
    #endregion
}
