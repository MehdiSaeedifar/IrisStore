using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region LinkViewModel
    public class LinkViewModel : IHaveCustomMappings
    {
        #region Properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string SlugUrl { get; set; }
        public string Image { get; set; }
        #endregion

        #region CreateMappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Page, LinkViewModel>()
                .ForMember(model => model.Image, opt => opt.Ignore());

            configuration.CreateMap<Post, LinkViewModel>();

            configuration.CreateMap<PostCategory, LinkViewModel>()
                .ForMember(link => link.Title, opt => opt.MapFrom(postCatagory => postCatagory.Name))
                .ForMember(link => link.SlugUrl, opt => opt.MapFrom(postCatagory => postCatagory.Name));
        }
        #endregion
    }
    #endregion
}
