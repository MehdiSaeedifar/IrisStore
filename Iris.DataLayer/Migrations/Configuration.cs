using System.Data.Entity.Migrations;
using System.Linq;
using Iris.DomainClasses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Iris.DataLayer.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            var optionsCount = context.SiteOptions.Count();

            if (optionsCount == 0)
            {
                context.SiteOptions.AddOrUpdate(
                 option => option.Key,
                 new SiteOption { Key = "SiteUrl" },
                 new SiteOption { Key = "SiteName" },
                 new SiteOption { Key = "SiteKeywords" },
                 new SiteOption { Key = "SiteDescription" },
                 new SiteOption { Key = "MailServerUrl" },
                 new SiteOption { Key = "MailServerPort" },
                 new SiteOption { Key = "MailServerUserName" },
                 new SiteOption { Key = "MailServerPassword" },
                 new SiteOption { Key = "ContactInfo" }
           );
            }


        }
    }
}
