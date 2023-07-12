
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
        Services = provider;
        AlertSvc = Services.GetService<IAlertService>();

        MainPage = new AppShell();
	}
}

