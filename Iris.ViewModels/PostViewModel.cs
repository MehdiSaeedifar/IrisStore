using System;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class PostViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string MetaDescription { get; set; }
        public string AuthorName { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public int ViewNumber { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public string SlugUrl { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
                .ForMember(postModel => postModel.AuthorName, opt => opt.MapFrom(post => post.PostedByUser.UserName))
                .ForMember(postModel => postModel.CategoryId, opt => opt.MapFrom(post => post.CategoryId))
                .ForMember(postModel => postModel.CategoryName, opt => opt.MapFrom(post => post.Category.Name));

            configuration.CreateMap<Page, PostViewModel>()
             .ForMember(postModel => postModel.CategoryId, opt => opt.Ignore())
             .ForMember(postModel => postModel.CategoryName, opt => opt.Ignore());
        }
    }
}
