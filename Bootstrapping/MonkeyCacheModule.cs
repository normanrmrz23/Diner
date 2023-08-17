using Autofac;
using MonkeyCache.SQLite;
using Diner.Persistence;

namespace Diner.Bootstrapping
{
    public class MonkeyCacheModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Barrel.ApplicationId = "diner";

            builder.RegisterInstance(Barrel.Current);

            builder.RegisterType<DefaultBackgroundTaskSchedulerFactory>().As<IBackgroundTaskSchedulerFactory>()
                .SingleInstance();

            builder
                .RegisterGeneric(typeof(MonkeyCacheRepository<>))
                .As(typeof(IRepository<>));
        }
    }
}
