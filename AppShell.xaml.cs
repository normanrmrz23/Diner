namespace Diner;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(Views.Lists), typeof(Views.Lists));
		Routing.RegisterRoute(nameof(Views.BusinessPage), typeof(Views.BusinessPage));
    }
}

