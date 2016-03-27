using AutoMapper;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class PageDataGridViewModel : PostDataGridViewModel
    {
        public int Order { get; set; }

        public override void CreateMappings(IConfiguration configuration)
        {
            base.CreateMappings(configuration);

            configuration.CreateMap<Page, PageDataGridViewModel>();
        }
    }
}
