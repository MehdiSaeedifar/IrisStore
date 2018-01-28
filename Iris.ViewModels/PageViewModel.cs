using System.Collections.Generic;
using AutoMapper;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region AddPageViewModel
    public class AddPageViewModel : AddPostViewModel
    {
        #region Constractors
        public AddPageViewModel()
        {
            Pages = new List<AddPageViewModel>();
        }
        #endregion

        #region Properties
        public int Order { get; set; }

        public IList<AddPageViewModel> Pages { get; set; }
        #endregion

        #region CreateMappings
        public override void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Page, AddPageViewModel>().ForMember(x => x.Tags, opt => opt.Ignore()).IncludeBase<Post, AddPostViewModel>();

            configuration.CreateMap<AddPageViewModel, Page>().ForMember(x=>x.Tags,opt=>opt.Ignore()).IncludeBase<AddPostViewModel, Post>();
        }
        #endregion
    }
    #endregion
}
