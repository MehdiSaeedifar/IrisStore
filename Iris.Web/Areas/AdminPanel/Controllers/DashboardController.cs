using System.Threading.Tasks;
using System.Web.Mvc;
using Iris.ServiceLayer.Contracts;

namespace Iris.Web.Areas.AdminPanel.Controllers
{
    #region DashboardController
    [Authorize(Roles = "Admin")]
    [RouteArea("AdminPanel", AreaPrefix = "Admin")]
    public partial class DashboardController : Controller
    {
        #region Fields
        private readonly IAdminPanelService _adminPanelService;
        #endregion

        #region Constructors
        public DashboardController(IAdminPanelService adminPanelService)
        {
            _adminPanelService = adminPanelService;
        }
        #endregion

        #region Index
        [Route()]
        public virtual async Task<ActionResult> Index()
        {
            return View(await _adminPanelService.GetDashboardStatistics());
        }
        #endregion
    }
    #endregion
}
