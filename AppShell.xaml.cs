namespace Diner;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(Views.ListsPage), typeof(Views.ListsPage));
		Routing.RegisterRoute(nameof(Views.BusinessPage), typeof(Views.BusinessPage));
    }
}

