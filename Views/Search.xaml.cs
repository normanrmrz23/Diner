namespace Diner.Views;

public partial class Search : ContentPage
{
	public Search()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        businessCollection.SelectedItem = null;
    }

}
