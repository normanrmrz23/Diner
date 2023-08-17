using System.Collections.Concurrent;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Serilog;

namespace Diner.Persistence
{
    public static class BackgroudTaskKeys
    {
        public const string Default = "default-background-scheduler";
        public const string Page = "page-background-scheduler";
    }
    public interface IBackgroundTaskSchedulerFactory
    {
        IScheduler Create(string key = BackgroudTaskKeys.Default, IScheduler cleanupScheduler = null);
    }

    public class DefaultBackgroundTaskSchedulerFactory : IBackgroundTaskSchedulerFactory, IDisposable
    {
        private readonly ConcurrentDictionary<string, EventLoopScheduler> _schedulers = new ConcurrentDictionary<string, EventLoopScheduler>();
        private readonly ILogger _logger;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        public DefaultBackgroundTaskSchedulerFactory(ILogger logger)
        {
            _logger = logger;
        }

        public IScheduler Create(string key = BackgroudTaskKeys.Default, IScheduler cleanupScheduler = null)
        {
            _logger.Verbose("Getting background scheduler '{key}'.", key);
            return _schedulers.GetOrAdd(key, k => CreateScheduler(k, cleanupScheduler));
        }

        public EventLoopScheduler CreateScheduler(string name = "occasional background tasks", IScheduler cleanupScheduler = null)
        {
            var result = new EventLoopScheduler(start =>
            {
                var t = new Thread(start)
                {
                    IsBackground = true,
                    Name = name
                };
                _logger.Verbose("Creating background scheduler '{name}' with id.", name, t.ManagedThreadId);
                return t;
            });

            cleanupScheduler = cleanupScheduler ?? Scheduler.Default;
            _disposables.Add(new ScheduledDisposable(cleanupScheduler, result));
            return result;
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _schedulers.Clear();
        }
    }
}
