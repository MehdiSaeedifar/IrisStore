using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;

namespace Iris.ViewModels
{
    public class FactorDataGridViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public FactorStatus Status { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Factor, FactorDataGridViewModel>();
        }
    }
}
