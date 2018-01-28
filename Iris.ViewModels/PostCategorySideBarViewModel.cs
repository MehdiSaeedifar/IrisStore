using System.Collections.Generic;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region PostCategorySideBarViewModel
    public class PostCategorySideBarViewModel : IHaveCustomMappings
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<LinkViewModel> Posts { get; set; }
        #endregion

        #region CreateMappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<PostCategory, PostCategorySideBarViewModel>();

        }
        #endregion
    }
    #endregion
}
