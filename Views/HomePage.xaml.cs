using Diner.ViewModels;

namespace Diner.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
