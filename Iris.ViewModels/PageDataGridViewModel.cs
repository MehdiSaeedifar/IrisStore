using AutoMapper;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region PageDataGridViewModel
    public class PageDataGridViewModel : PostDataGridViewModel
    {
        #region Properties
        public int Order { get; set; }
        #endregion

        #region CreateMappings
        public override void CreateMappings(IConfiguration configuration)
        {
            base.CreateMappings(configuration);

            configuration.CreateMap<Page, PageDataGridViewModel>();
        }
        #endregion
    }
    #endregion
}
