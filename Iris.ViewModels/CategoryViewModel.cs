using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region CategoryViewModel
    public class CategoryViewModel : IHaveCustomMappings
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Category, CategoryViewModel>();
            configuration.CreateMap<PostCategory, CategoryViewModel>();
            configuration.CreateMap<CategoryViewModel, GroupViewModel>();
        }
        #endregion
    }
    #endregion
}
