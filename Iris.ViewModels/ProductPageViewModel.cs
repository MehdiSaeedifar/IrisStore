using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class ProductPageViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:###,###.####}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string Description { get; set; }
        public IList<ProductPageImageViewModel> Images { get; set; }
        public IList<ProductPagePriceViewModel> Prices { get; set; }
        public string SlugUrl { get; set; }
        public string MetaDescription { get; set; }
        public double? AverageRating { get; set; }
        public int ViewNumber { get; set; }
        public IList<CategoryViewModel> Categories { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Product, ProductPageViewModel>()
                .ForMember(productModel => productModel.Name, opt => opt.MapFrom(product => product.Title))

                .ForMember(productModel => productModel.Price,
                    opt =>
                        opt.MapFrom(
                            product =>
                                product.Prices.OrderByDescending(price => price.Date)
                                    .Select(price => price.Price)
                                    .FirstOrDefault()))

                .ForMember(productModel => productModel.Description, opt => opt.MapFrom(product => product.Body))

                .ForMember(productModel => productModel.Images,
                    opt => opt.MapFrom(product => product.Images.OrderBy(image => image.Order)))

                .ForMember(productModel => productModel.Prices,
                    opt => opt.MapFrom(product => product.Prices.OrderBy(price => price.Date)));

        }
    }

    public class ProductPageImageViewModel : IHaveCustomMappings
    {
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductImage, ProductPageImageViewModel>();
        }
    }

    public class ProductPagePriceViewModel : IHaveCustomMappings
    {
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductPrice, ProductPagePriceViewModel>()
                .ForMember(priceModel => priceModel.DateTime, opt => opt.MapFrom(productPrice => productPrice.Date));
        }
    }

}
