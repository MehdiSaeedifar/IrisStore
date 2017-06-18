using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class ProductOrderViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:###,###.####}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        public int Count { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Product, ProductOrderViewModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Title))
                .ForMember(d => d.Price,
                    opt => opt.MapFrom(s => s.Prices.OrderByDescending(p => p.Date).FirstOrDefault().Price));
        }
    }
}
