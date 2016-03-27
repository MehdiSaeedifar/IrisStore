using System.Threading.Tasks;
using System.Web.Mvc;
using Iris.ServiceLayer.Contracts;

namespace Iris.Web.Areas.AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    [RouteArea("AdminPanel", AreaPrefix = "Admin")]
    public partial class DashboardController : Controller
    {
        private readonly IAdminPanelService _adminPanelService;

        public DashboardController(IAdminPanelService adminPanelService)
        {
            _adminPanelService = adminPanelService;
        }

        [Route()]
        public virtual async Task<ActionResult> Index()
        {
            return View(await _adminPanelService.GetDashboardStatistics());
        }
    }
}