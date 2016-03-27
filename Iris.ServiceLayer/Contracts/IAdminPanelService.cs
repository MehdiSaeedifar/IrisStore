using System.Threading.Tasks;
using Iris.ViewModels;

namespace Iris.ServiceLayer.Contracts
{
    public interface IAdminPanelService
    {
        Task<AdminDashboardViewModel> GetDashboardStatistics();
    }
}
