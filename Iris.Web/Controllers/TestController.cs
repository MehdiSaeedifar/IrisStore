using System.Web.Mvc;

namespace Iris.Web.Controllers
{
    [RoutePrefix("Test")]
    public partial class TestController : Controller
    {
        [Route("Index")]
        public virtual ActionResult Index()
        {
            //IoC.Container.GetInstance<IApplicationUserManager>().SeedDatabase();

            //
            //IoC.Container.GetInstance<IUnitOfWork>().Set<SiteOption>().AddOrUpdate(
            //  option => new { option.Key, option.Value },
            //  new SiteOption { Key = "SiteUrl" },
            //  new SiteOption { Key = "SiteName" },
            //  new SiteOption { Key = "SiteKeywords" },
            //  new SiteOption { Key = "SiteDescription" },
            //  new SiteOption { Key = "MailServerUrl" },
            //  new SiteOption { Key = "MailServerPort" },
            //  new SiteOption { Key = "MailServerUserName" },
            //  new SiteOption { Key = "MailServerPassword" },
            //  new SiteOption { Key = "ContactInfo" }
            //);

            //IoC.Container.GetInstance<IUnitOfWork>().SaveAllChanges();
            return View();
        }
    }
}