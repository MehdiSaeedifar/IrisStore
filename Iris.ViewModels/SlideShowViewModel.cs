using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region SlideShowViewModel
    public class SlideShowViewModel : IHaveCustomMappings
    {
        #region Constractors
        public SlideShowViewModel()
        {
            Order = 0;
        }
        #endregion

        #region Properties
        public int? Id { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }

        [Display(Name = "لینک")]
        public string Link { get; set; }

        [Display(Name = "ترتیب")]
        public int Order { get; set; }

        public IList<SlideShowViewModel> SlideShowImages { get; set; }
        #endregion

        #region CreateMappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<SlideShowViewModel, SlideShowImage>();


            configuration.CreateMap<SlideShowImage, SlideShowViewModel>()
                .ForMember(slideShow => slideShow.SlideShowImages, opt => opt.Ignore());
        }
        #endregion
    }
    #endregion
}
