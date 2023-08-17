using Diner.ViewModels;

namespace Diner.Views;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        businessCollection.SelectedItem = null;
    }

}
