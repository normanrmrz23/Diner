
using Autofac;
using Diner.Bootstrapping;
using Diner.Persistence;
using Diner.Services;
using Diner.Views;

[assembly: ExportFont("fa-regular.ttf", Alias = "FontAwesome")]
//[assembly: ExportFont("fa-solid-900.ttf", Alias = "FontAwesome")]
//[assembly: ExportFont("MaterialIcons-Regular.ttf", Alias = "FontAwesome")]

namespace Diner;
public partial class App : Application
{
    public static IServiceProvider Services;
    public static IAlertService AlertSvc;

    public App(IServiceProvider provider)
	{
		InitializeComponent();
        Configure(_ => { });

        Services = provider;
        AlertSvc = Services.GetService<IAlertService>();
        MainPage = new AppShell();
        
	}

    public Autofac.IContainer Container { get; private set; }

    public static Registrant[] BootstrappedAssemblies => new[]
    {
            //new Registrant(typeof(MainPageViewModel).Assembly),
            //new Registrant(typeof(Navigator).Assembly),
            new Registrant(typeof(MonkeyCacheRepository<>).Assembly),
            //new Registrant(typeof(IDevicePlatformInfo).Assembly),
            //new Registrant(typeof(IImageStore).Assembly),
            //new Registrant(typeof(INetworkReachablePulse).Assembly),
            new Registrant(typeof(ISystemIODirectory).Assembly, typeof(SingletonAttribute), typeof(IgnoreDefaultConventionAttribute)),
        };

    public void Configure(Action<ContainerBuilder> externalConfigAction)
    {
        try
        {
            var cb = new ContainerBuilder();
            cb.BootstrapTypes(BootstrappedAssemblies);
            externalConfigAction(cb);

           // cb.RegisterType<Db>().As<IDb>().SingleInstance();

            Container = cb.Build();
           // var _ = Container.Resolve<IBackgroundTaskSchedulerFactory>(); // ensure singleton is scoped to app not user lifetime scope.
        }
        catch (Exception e)
        {
            //Log.Logger.Error(e, "Failure during AutoFac initialization");
            throw;
        }
    }
}

