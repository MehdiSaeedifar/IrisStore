using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;
using Utilities;

namespace Iris.ViewModels
{
    public class AddProductViewModel : IHaveCustomMappings
    {

        public AddProductViewModel()
        {
            Categories = new List<string>();
            Images = new List<AddProductImageViewModel>();
            SeoFields = new MetaTagsViewModel();
            Prices = new List<AddProductPriceViewModel>();
        }

        public int? Id { get; set; }

        [Display(Name = "نام کالا")]
        [Required(ErrorMessage = "لطفا نام کالا را وارد نمایید")]
        [StringLength(400, MinimumLength = 2, ErrorMessage = "نام کالا باید بین 2 تا 400 حرف باشد")]
        public string Name { get; set; }

        [Display(Name = "وضعیت کالا")]
        [Required(ErrorMessage = "لطفا وضعیت کالا را انتخاب نمایید")]
        public ProductStatus ProductStatus { get; set; }

        [Display(Name = "قیمت")]
        public decimal? Price { get; set; }

        [Display(Name = "گروه‌ها")]
        [Required(ErrorMessage = "لطفا حداقل یک گروه را انتخاب نمایید")]
        public List<string> Categories { get; set; }

        [Display(Name = "توضیحات")]
        [AllowHtml]
        public string Description { get; set; }

        public List<AddProductImageViewModel> Images { get; set; }
        public List<AddProductPriceViewModel> Prices { get; set; }

        public MetaTagsViewModel SeoFields { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {

            configuration.CreateMap<Product, AddProductViewModel>()
                .ForMember(productModel => productModel.SeoFields, opt => opt.MapFrom(productModel => new MetaTagsViewModel
                {
                    SlugUrl = productModel.SlugUrl,
                    MetaDescription = productModel.MetaDescription,
                }))
                .ForMember(p => p.Categories, opt => opt.MapFrom(x => x.Categories.Select(c => c.Name)))
                .ForMember(productModel => productModel.Name, opt => opt.MapFrom(product => product.Title))
                .ForMember(productModel => productModel.Description, opt => opt.MapFrom(product => product.Body))
                .ForMember(productModel => productModel.Images, opt => opt.MapFrom(product => product.Images.OrderBy(image => image.Order)))

            .ForMember(productModel => productModel.Prices,
            opt => opt.MapFrom(product => product.Prices.OrderByDescending(price => price.Date)));




            configuration.CreateMap<AddProductViewModel, Product>()
                .ForMember(product => product.Categories,
                    opt => opt.MapFrom(productModel => productModel
                    .Categories.Select(x => new Category() { Name = x }).ToList()))

                    .ForMember(product => product.Title, opt => opt.MapFrom(productModel => productModel.Name))

                    .ForMember(product => product.Body, opt => opt.MapFrom(productModel => productModel.Description))
                //.ForMember(product => product.Images, opt => opt.Ignore())
                //.ForMember(product => product.Prices, opt => opt.Ignore())
                .ForMember(product => product.Tags, opt => opt.Ignore())
                .ForMember(product => product.PostedByUser, opt => opt.Ignore())
                .ForMember(product => product.PostedDate, opt => opt.Ignore())
                .ForMember(product => product.EditedDate, opt => opt.Ignore())
                .ForMember(product => product.PostedByUserId, opt => opt.Ignore())
                .ForMember(product => product.EditedDate, opt => opt.Ignore())

                .ForMember(product => product.SlugUrl, opt => opt.ResolveUsing(productModel =>
                SeoHelpers.GenerateSlug(string.IsNullOrWhiteSpace(productModel.SeoFields.SlugUrl) ?
                    productModel.Name :
                    productModel.SeoFields.SlugUrl)
                    ))

                .ForMember(product => product.MetaDescription, opt => opt.ResolveUsing(productModel => SeoHelpers.GenerateMetaDescription(!string.IsNullOrWhiteSpace(productModel.SeoFields.MetaDescription) ? productModel.SeoFields.MetaDescription : productModel.Description)))

                .ForMember(product => product.Images, opt => opt.ResolveUsing(productModel =>
                {
                    var i = 0;
                    productModel.Images.ForEach(image =>
                    {
                        image.Order = i++;
                    });
                    return productModel.Images;
                }))

                 .ForMember(product => product.Prices, opt => opt.ResolveUsing(productModel =>
                 {
                     if (productModel.Price.HasValue)
                     {
                         productModel.Prices.Add(new AddProductPriceViewModel
                         {
                             Price = productModel.Price.Value,
                             Date = DateTime.Now
                         });
                     }
                     return productModel.Prices;
                 }))


                .ForMember(product => product.Tags, opt => opt.Ignore());


        }
    }

    public class AddProductImageViewModel : IHaveCustomMappings
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Url { get; set; }
        public int Size { get; set; }
        public string DeleteUrl { get; set; }
        public int? ProductId { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductImage, AddProductImageViewModel>();

            configuration.CreateMap<AddProductImageViewModel, ProductImage>()
                .ForMember(image => image.Product, opt => opt.Ignore());
        }
    }

    public class MetaTagsViewModel : IHaveCustomMappings
    {
        [Display(Name = "نام URL")]
        public string SlugUrl { get; set; }

        //[Display(Name = "برچسب ها")]
        public string[] Tags { get; set; }

        [Display(Name = "توضیحات")]
        public string MetaDescription { get; set; }
        public virtual void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<MetaTagsViewModel, Post>().ForMember(x=>x.Tags,opt=>opt.Ignore());
        }
    }

    public class AddProductPriceViewModel : IHaveCustomMappings
    {

        public int? Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:###,###.####}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public int? ProductId { get; set; }
        public  void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductPrice, AddProductPriceViewModel>();

            configuration.CreateMap<AddProductPriceViewModel, ProductPrice>()
                .ForMember(price => price.Product, opt => opt.Ignore());
        }
    }


}