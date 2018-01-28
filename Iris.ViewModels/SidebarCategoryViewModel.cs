using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region SidebarCategoryViewModel
    public class SidebarCategoryViewModel : IHaveCustomMappings
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductsCount { get; set; }
        #endregion

        #region CreateMappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Category, SidebarCategoryViewModel>()
                .ForMember(categoryModel => categoryModel.ProductsCount, opt => opt.MapFrom(product => product.Products.Count));
        }
        #endregion
    }
    #endregion
}
