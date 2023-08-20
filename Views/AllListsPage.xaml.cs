using Diner.ViewModels;

namespace Diner.Views;

public partial class AllListsPage : ContentPage
{
    public AllListsPage(AllListsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        listCollection.SelectedItem = null;
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (listCollection.SelectedItem != null)
        {
            listCollection.SelectedItem = null;
        }

    }
}