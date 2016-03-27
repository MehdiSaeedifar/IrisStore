using System;
using System.IO;
using System.Web;
using DNTScheduler;

namespace Iris.Web.WebTasks
{
    public class CleanTempDirectoryTask : ScheduledTaskTemplate
    {
        /// <summary>
        /// اگر چند جاب در يك زمان مشخص داشتيد، اين خاصيت ترتيب اجراي آن‌ها را مشخص خواهد كرد
        /// </summary>
        public override int Order => 0;

        public override bool RunAt(DateTime utcNow)
        {
            if (this.IsShuttingDown || this.Pause)
                return false;

            var now = utcNow.AddHours(3.5);

            return now.Hour == 3 &&
                     now.Minute == 1 && now.Second == 1;
        }

        public override void Run()
        {
            if (this.IsShuttingDown || this.Pause)
                return;

            var path = Path.Combine(HttpRuntime.AppDomainAppPath, "Content\\tmp");

            foreach (var file in Directory.GetFiles(path))
            {
                File.Delete(file);
            }

        }

        public override string Name => "CleanTempDirectoryTask";
    }

}