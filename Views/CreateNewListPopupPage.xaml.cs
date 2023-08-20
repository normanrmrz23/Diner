using CommunityToolkit.Maui.Views;
using Diner.ViewModels;

namespace Diner.Views;

public partial class CreateNewListPopupPage : Popup
{
	public CreateNewListPopupPage(CreateNewListPopupPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        Size = new(300, 250);
    }
}
