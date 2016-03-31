using System.Web.Mvc;
using Iris.ServiceLayer.Contracts;
using Iris.Web.Caching;
using Iris.Web.DependencyResolution;

namespace Iris.Web.Filters
{
    public class SiteNameActionFilter : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var siteSettings = IoC.Container.GetInstance<ICacheService>().GetSiteSettings();

            filterContext.Controller.ViewBag.SiteName = siteSettings.SiteName;
            filterContext.Controller.ViewBag.SiteDescription = siteSettings.SiteDescription;
            filterContext.Controller.ViewBag.ContactInfo = siteSettings.ContactInfo;
        }

    }
}
