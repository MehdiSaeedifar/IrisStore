# DNTScheduler
DNTScheduler is a super simple ASP.NET's background tasks runner and scheduler.
It's compatible with .NET 4.0+

## Usage
* To start using DNTScheduler package, download its source code from the GitHub (https://github.com/VahidN/DNTScheduler/)
  and then take a look at these files:
  - How to define a new task:
       DNTScheduler.TestWebApplication\WebTasks\DoBackupTask.cs
  - How to register it:
       DNTScheduler.TestWebApplication\WebTasks\ScheduledTasksRegistry.cs
  - How to initialize it:
       DNTScheduler.TestWebApplication\Global.asax.cs
  - How to get the list of active tasks:
       DNTScheduler.TestWebApplication\Default.aspx.cs