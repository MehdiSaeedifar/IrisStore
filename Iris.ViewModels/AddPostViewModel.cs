using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutoMapper;
using Iris.DomainClasses;
using Utilities;

namespace Iris.ViewModels
{
    public class AddPostViewModel : MetaTagsViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "متن")]
        [AllowHtml]
        public string Body { get; set; }

        [Display(Name = "گروه")]
        public int? CategoryId { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }

        public override void CreateMappings(IConfiguration configuration)
        {
            base.CreateMappings(configuration);

            configuration.CreateMap<Post, AddPostViewModel>().ForMember(postModel => postModel.Tags, opt => opt.Ignore()).IncludeBase<Post, MetaTagsViewModel>();

            configuration.CreateMap<AddPostViewModel, Post>()
                .ForMember(postModel => postModel.SlugUrl, opt => opt.ResolveUsing(postModel =>
                    SeoHelpers.GenerateSlug(string.IsNullOrWhiteSpace(postModel.SlugUrl)
                        ? postModel.Title
                        : postModel.SlugUrl)
                    ))

                     .ForMember(postModel => postModel.MetaDescription, opt => opt.ResolveUsing(postModel => SeoHelpers.GenerateMetaDescription(!string.IsNullOrWhiteSpace(postModel.MetaDescription) ? postModel.MetaDescription : postModel.Body)))

                      .ForMember(postModel => postModel.Tags, opt => opt.Ignore()).IncludeBase<MetaTagsViewModel, Post>();
        }
    }
}
