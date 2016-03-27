using System;

namespace Scheduler
{
    /// <summary>
    /// Scheduled task's contract.
    /// </summary>
    public abstract class ScheduledTaskTemplate
    {
        /// <summary>
        /// Name of the task.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Priority of the task.
        /// </summary>
        public abstract int Order { get; }

        /// <summary> 
        /// If the RunAt method returns true, the Run method will be executed.
        /// utcNow is expressed as the Coordinated Universal Time (UTC).
        /// It will be called every one second.
        /// </summary>
        /// <param name="utcNow">Expressed as the Coordinated Universal Time (UTC).</param>
        public abstract bool RunAt(DateTime utcNow);

        /// <summary>
        /// Scheduled task's logic.
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// IsRunning will be set by the coordinator.
        /// </summary>
        public bool IsRunning { set; get; }

        /// <summary>
        /// Is ASP.Net app domain tearing down?
        /// If set to true by the coordinator, the task should cleanup and return.
        /// </summary>
        public bool IsShuttingDown { set; get; }

        /// <summary>
        /// Pause the task.
        /// </summary>
        public bool Pause { set; get; }

        /// <summary>
        /// Task's last run time.
        /// </summary>
        public DateTime? LastRun { get; set; }

        /// <summary>
        /// Status of last run.
        /// </summary>
        public bool? IsLastRunSuccessful { get; set; }
    }
}