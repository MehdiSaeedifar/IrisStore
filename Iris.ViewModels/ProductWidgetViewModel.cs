using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class ProductWidgetViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:###,###.####}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        public string Image { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string SlugUrl { get; set; }
        public string Category { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Product, ProductWidgetViewModel>()
                .ForMember(productModel => productModel.Name, opt => opt.MapFrom(product => product.Title))

                .ForMember(productModel => productModel.Price,

                    opt =>
                        opt.MapFrom(
                            product =>
                                product.Prices.OrderByDescending(price => price.Date)
                                    .Select(price => price.Price)
                                    .FirstOrDefault()))

                .ForMember(productModel => productModel.Image,
                    opt =>
                        opt.MapFrom(
                            product =>
                                product.Images.OrderBy(image => image.Order).Select(image => image.ThumbnailUrl).FirstOrDefault()));
        }
    }
}
