namespace Diner.Views;

public partial class AllListsPage : ContentPage
{
    public AllListsPage()
    {
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        listCollection.SelectedItem = null;
    }
}