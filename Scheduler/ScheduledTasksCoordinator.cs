using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Scheduler
{
    /// <summary>
    /// Scheduled Tasks Manager
    /// </summary>
    public class ScheduledTasksCoordinator : System.Web.Hosting.IRegisteredObject, IDisposable
    {
        /// <summary>
        /// The only instance of the ScheduledTasksCoordinator.
        /// </summary>
        public static ScheduledTasksCoordinator Current = new ScheduledTasksCoordinator();

        private readonly List<ScheduledTaskTemplate> _tasks = new List<ScheduledTaskTemplate>();
        private readonly JobsRunnerTimer _timer = new JobsRunnerTimer();
        private Thread _taskThread;
        private bool _isShuttingDown;
        private int _disposed;
        private readonly object _syncLock = new object();
        const int TimeToFinish = 30 * 1000; // the 30 seconds is for the entire app to tie up what it's doing.

        /// <summary>
        /// Scheduled Tasks Manager
        /// </summary>
        public ScheduledTasksCoordinator()
        {
            System.Web.Hosting.HostingEnvironment.RegisterObject(this);
            _timer.Start();
        }

        /// <summary>
        /// Gets the list of the scheduled tasks.
        /// </summary>
        public ScheduledTaskTemplate[] ScheduledTasks
        {
            get { return _tasks.ToArray(); }
        }

        /// <summary>
        /// Fires on unhandled exceptions.
        /// </summary>
        public Action<Exception, ScheduledTaskTemplate> OnUnexpectedException { set; get; }

        /// <summary>
        /// Adds a new scheduled task.
        /// </summary>
        /// <param name="scheduledTask">new task</param>
        public void AddScheduledTask(ScheduledTaskTemplate scheduledTask)
        {
            _tasks.Add(scheduledTask);
        }

        /// <summary>
        /// Adds new scheduled tasks.
        /// </summary>
        /// <param name="scheduledTasks">Tasks list</param>
        public void AddScheduledTasks(params ScheduledTaskTemplate[] scheduledTasks)
        {
            foreach (var task in scheduledTasks)
            {
                _tasks.Add(task);
            }
        }

        /// <summary>
        /// Starts TimerCallback.
        /// </summary>
        public void Start()
        {
            _timer.OnTimerCallback = () =>
            {
                var now = DateTime.UtcNow;
                var taskToRun = _tasks.Where(x => !x.IsRunning && x.RunAt(now)).OrderBy(x => x.Order).ToList();
                if (_isShuttingDown || !taskToRun.Any())
                    return;

                _taskThread = new Thread(() => taskAction(taskToRun))
                {
                    Priority = ThreadPriority.BelowNormal,
                    IsBackground = true
                };
                _taskThread.Start();
            };
        }

        private void taskAction(IEnumerable<ScheduledTaskTemplate> taskToRun)
        {
            foreach (var scheduledTask in taskToRun)
            {
                try
                {
                    scheduledTask.IsRunning = true;
                    scheduledTask.LastRun = DateTime.UtcNow;

                    scheduledTask.Run();

                    scheduledTask.IsLastRunSuccessful = true;
                }
                catch (Exception ex)
                {
                    scheduledTask.IsLastRunSuccessful = false;
                    OnUnexpectedException(ex, scheduledTask);
                }
                finally
                {
                    scheduledTask.IsRunning = false;
                }
            }
        }

        /// <summary>
        /// Stops the scheduler.
        /// </summary>
        public void Stop()
        {
            Dispose();
        }

        /// <summary>
        /// Call if the app is shutting down. Should only be called by the ASP.Net container.
        /// </summary>
        /// <param name="immediate">ASP.Net sets this to false first, then to true the second
        /// call 30 seconds later.</param>
        public void Stop(bool immediate)
        {
            // See: http://haacked.com/archive/2011/10/16/the-dangers-of-implementing-recurring-background-tasks-in-asp-net.aspx

            if (_isShuttingDown)
                return;

            lock (_syncLock)
            {
                _isShuttingDown = true;

                foreach (var scheduledTask in _tasks)
                {
                    scheduledTask.IsShuttingDown = true;
                }

                var timeOut = TimeToFinish;
                while (_tasks.Any(x => x.IsRunning) && timeOut >= 0)
                {
                    // still running ...
                    Thread.Sleep(50);
                    timeOut -= 50;
                }
                Dispose();
            }
        }

        /// <summary>
        /// Stops the scheduler.
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.Increment(ref _disposed) != 1)
                return;

            Stop();
            System.Web.Hosting.HostingEnvironment.UnregisterObject(this);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Stops the scheduler.
        /// </summary>
        ~ScheduledTasksCoordinator()
        {
            Dispose();
        }
    }
}