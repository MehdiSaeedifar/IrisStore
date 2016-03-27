using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Iris.DataLayer;
using Iris.Web.ModelBinders;
using Iris.Web.WebTasks;
using StructureMap.Web.Pipeline;

namespace Iris.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                System.Web.Mvc.ModelBinders.Binders.Add(typeof(decimal?), new DecimalBinder());
                System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime), new PersianDateModelBinder());

                AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);


                BundleConfig.RegisterBundles(BundleTable.Bundles);

                AutoMapperConfig.Config();

                //Database.SetInitializer<ApplicationDbContext>(null);

                Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext,
                    DataLayer.Migrations.Configuration>());

                DbInterception.Add(new YeKeInterceptor());

                ScheduledTasksRegistry.Init();

                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(new RazorViewEngine());

                MvcHandler.DisableMvcResponseHeader = true;
            }
            catch
            {
                HttpRuntime.UnloadAppDomain(); // سبب ری استارت برنامه و آغاز مجدد آن با درخواست بعدی می‌شود
                throw;
            }
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            app?.Context?.Response.Headers.Remove("Server");
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }

        protected void Application_End()
        {
            ScheduledTasksRegistry.End();
            //نکته مهم این روش نیاز به سرویس پینگ سایت برای زنده نگه داشتن آن است
            ScheduledTasksRegistry.WakeUp(IrisApp.GetSiteRootUrl());
        }

    }

    public static class IrisApp
    {
        public static string GetSiteRootUrl()
        {
            return ConfigurationManager.AppSettings["SiteRootUrl"];
        }
    }
}
