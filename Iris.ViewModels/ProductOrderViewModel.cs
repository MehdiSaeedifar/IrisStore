using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region ProductOrderViewModel
    public class ProductOrderViewModel : IHaveCustomMappings
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:###,###}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal Discount { get; set; }
        public int Count { get; set; }
        public IList<ProductDiscountViewModel> Discounts { get; set; }

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
            configuration.CreateMap<Product, ProductOrderViewModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Title))
                .ForMember(d => d.Price,
                    opt => opt.MapFrom(s => s.Prices.OrderByDescending(p => p.Date).FirstOrDefault().Price))

                .ForMember(productModel => productModel.Discount,
                    opt =>
                        opt.MapFrom(
                            product =>
                                product.Discounts.OrderByDescending(discount => discount.StartDate)
                                    .Select(discount => discount.Discount)
                                    .FirstOrDefault()));
        }
        #endregion
    }
    #endregion

    #region ProductDiscountViewModel
    public class ProductDiscountViewModel : IHaveCustomMappings
    {
        #region Properties
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        #endregion

        #region CreateMappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ProductDiscount, ProductDiscountViewModel>()
               .ForMember(discountModel => discountModel.StartDate, opt => opt.MapFrom(productdiscount => productdiscount.StartDate))
                           .ForMember(discountModel => discountModel.EndDate, opt => opt.MapFrom(productdiscount => productdiscount.EndDate));

            configuration.CreateMap<ProductDiscountViewModel, ProductDiscount>()
                .ForMember(discount => discount.Product, opt => opt.Ignore());
        }
        #endregion
    }
    #endregion
}
