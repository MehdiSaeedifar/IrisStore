using System;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class PagedListPostViewModel:IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SlugUrl { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public DateTime PostedDate { get; set; }
        public int ViewNumber { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, PagedListPostViewModel>();
        }
    }
}
