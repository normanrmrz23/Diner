namespace Diner.Views;

public partial class AllLists : ContentPage
{
    public AllLists()
    {
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        listCollection.SelectedItem = null;
    }
}