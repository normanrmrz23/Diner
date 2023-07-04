namespace Diner.Views;

public partial class Feed : ContentPage
{
	public Feed()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        listCollection.SelectedItem = null;
    }
}
