using System.Linq;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region ProductDataGridViewModel
    public class ProductDataGridViewModel : IHaveCustomMappings
    {
        #region Properties
        public int Id { get; set; }
        public string Title { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        //public int ViewNumber { get; set; }
        #endregion

        #region CreateMappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Product, ProductDataGridViewModel>()
                 .ForMember(pModel => pModel.Price,
                    opt =>
                        opt.MapFrom(
                            price => price.Prices.OrderByDescending(p => p.Date).Select(p => p.Price).FirstOrDefault()))
                .ForMember(pModel => pModel.Discount,
                    opt =>
                        opt.MapFrom(
                            discount => discount.Discounts.OrderByDescending(p => p.StartDate).Select(p => p.Discount).FirstOrDefault()));


            //.ForMember(pModel => pModel.ProductStatus, opt =>
            //    opt.ResolveUsing(price => price.ProductStatus.GetEnumDescription()));

        }
        #endregion
    }
    #endregion
}
