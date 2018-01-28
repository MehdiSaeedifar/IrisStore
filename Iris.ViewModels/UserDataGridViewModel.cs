using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region UserDataGridViewModel
    public class UserDataGridViewModel : IHaveCustomMappings
    {
        #region Properties
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        #endregion

        #region CreateMappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ApplicationUser, UserDataGridViewModel>();
        }
        #endregion
    }
    #endregion
}
