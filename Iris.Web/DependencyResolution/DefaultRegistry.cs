// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Data.Entity;
using System.Security.Principal;
using System.Web;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer;
using Iris.ServiceLayer.Contracts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Web;

namespace Iris.Web.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {

            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });

            For<HttpContextBase>()
                .HybridHttpOrThreadLocalScoped()
                .Use(() => new HttpContextWrapper(HttpContext.Current));

            For<IIdentity>().Use(() => (HttpContext.Current != null && HttpContext.Current.User != null) ? HttpContext.Current.User.Identity : null);

            For<IUnitOfWork>()
                .HybridHttpOrThreadLocalScoped()
                .Use<ApplicationDbContext>();
            // Remove these 2 lines if you want to use a connection string named connectionString1, defined in the web.config file.
            //.Ctor<string>("connectionString")
            //.Is("Data Source=(local);Initial Catalog=TestDbIdentity;Integrated Security = true");

            For<ApplicationDbContext>().HybridHttpOrThreadLocalScoped()
               .Use(context => (ApplicationDbContext)context.GetInstance<IUnitOfWork>());
            For<DbContext>().HybridHttpOrThreadLocalScoped()
               .Use(context => (ApplicationDbContext)context.GetInstance<IUnitOfWork>());

            For<IUserStore<ApplicationUser, int>>()
                .HybridHttpOrThreadLocalScoped()
                .Use<CustomUserStore>();

            For<IRoleStore<CustomRole, int>>()
                .HybridHttpOrThreadLocalScoped()
                .Use<RoleStore<CustomRole, int, CustomUserRole>>();

            For<IAuthenticationManager>()
                  .Use(() => HttpContext.Current.GetOwinContext().Authentication);

            For<IApplicationSignInManager>()
                  .HybridHttpOrThreadLocalScoped()
                  .Use<ApplicationSignInManager>();

            For<IApplicationRoleManager>()
                  .HybridHttpOrThreadLocalScoped()
                  .Use<ApplicationRoleManager>();

            // map same interface to different concrete classes
            For<IIdentityMessageService>().Use<SmsService>();
            For<IIdentityMessageService>().Use<EmailService>();

            For<IApplicationUserManager>().HybridHttpOrThreadLocalScoped()
               .Use<ApplicationUserManager>()
               .Ctor<IIdentityMessageService>("smsService").Is<SmsService>()
               .Ctor<IIdentityMessageService>("emailService").Is<EmailService>()
               .Setter<IIdentityMessageService>(userManager => userManager.SmsService).Is<SmsService>()
               .Setter<IIdentityMessageService>(userManager => userManager.EmailService).Is<EmailService>();

            For<ApplicationUserManager>().HybridHttpOrThreadLocalScoped()
               .Use(context => (ApplicationUserManager)context.GetInstance<IApplicationUserManager>());

            For<ICustomRoleStore>()
                  .HybridHttpOrThreadLocalScoped()
                  .Use<CustomRoleStore>();

            For<ICustomUserStore>()
                  .HybridHttpOrThreadLocalScoped()
                  .Use<CustomUserStore>();

            //config.For<IDataProtectionProvider>().Use(()=> app.GetDataProtectionProvider()); // In Startup class

            For<IProductService>().Use<ProductService>();
            For<ICategoryService>().Use<CategoryService>();
            For<ISlideShowImageService>().Use<SlideShowImageService>();
            For<IPageService>().Use<PageService>();
            For<IPostService>().Use<PostService>();
            For<IPostCategoryService>().Use<PostCategoryService>();
            For<ISiteSettingService>().Use<SiteSettingService>();
            For<IAdminPanelService>().Use<AdminPanelService>();
            For<ISiteMapService>().Use<SiteMapService>();
        }

        #endregion
    }
}