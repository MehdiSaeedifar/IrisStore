using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class SidebarCategoryViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductsCount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Category, SidebarCategoryViewModel>()
                .ForMember(categoryModel => categoryModel.ProductsCount, opt => opt.MapFrom(product => product.Products.Count));
        }
    }
}
