using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class UserDataGridViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ApplicationUser, UserDataGridViewModel>();
        }
    }
}
