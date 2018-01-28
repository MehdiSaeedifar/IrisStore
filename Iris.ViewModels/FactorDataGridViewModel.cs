using System;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    #region FactorDataGridViewModel
    public class FactorDataGridViewModel : IHaveCustomMappings
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public FactorStatus Status { get; set; }
        #endregion

        #region CreateMappings
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Factor, FactorDataGridViewModel>();
        }
        #endregion
    }
    #endregion
}
