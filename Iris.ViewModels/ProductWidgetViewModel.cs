using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;
using System.Collections.Generic;
using System;

namespace Iris.ViewModels
{
    #region ProductWidgetViewModel
    public class ProductWidgetViewModel : IHaveCustomMappings
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal Discount { get; set; }
        public string Image { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string SlugUrl { get; set; }
        public string Category { get; set; }

        public IList<ProductPageDiscountWidgetViewModel> Discounts { get; set; }

        #region Calculator Properties
        /// <summary>
        /// Calculator Discounts
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CalcDiscount { get { return (Price - ((Price * Discount) / 100)); } }
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal CalcDiscountFee { get { return (((Price * Discount) / 100)); } }
        #endregion

        #endregion

        #region CreateMappings
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

                                      .ForMember(productModel => productModel.Discount,

                    opt =>
                        opt.MapFrom(
                            product =>
                                product.Discounts.OrderByDescending(discount => discount.StartDate)
                                    .Select(discount => discount.Discount)
                                    .FirstOrDefault()))

                .ForMember(productModel => productModel.Image,
                    opt =>
                        opt.MapFrom(
                            product =>
                                product.Images.OrderBy(image => image.Order).Select(image => image.ThumbnailUrl).FirstOrDefault()));
        }
        #endregion
    }
    #endregion

    #region ProductPageDiscountWidgetViewModel
    public class ProductPageDiscountWidgetViewModel : IHaveCustomMappings
    {
        #region Properties
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        #endregion

        #region CreateMappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductDiscount, ProductPageDiscountWidgetViewModel>()
               .ForMember(discountModel => discountModel.StartDate, opt => opt.MapFrom(productdiscount => productdiscount.StartDate))
                           .ForMember(discountModel => discountModel.EndDate, opt => opt.MapFrom(productdiscount => productdiscount.EndDate));
        }
        #endregion
    }
    #endregion
}
