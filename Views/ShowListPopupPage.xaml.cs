using CommunityToolkit.Maui.Views;
using Diner.ViewModels;

namespace Diner.Views;

public partial class ShowListPopupPage : Popup
{
	public ShowListPopupPage(ShowListPopupPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        Size = new(300, 500);
    }
}
