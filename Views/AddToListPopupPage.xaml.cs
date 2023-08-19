using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using Diner.Models;
using Diner.Services;
using Diner.ViewModels;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Linq;
using Yelp.Api.Models;

namespace Diner.Views;

public partial class AddToListPopupPage : Popup
{
    public AddToListPopupPage(AddToListPopupPageViewModel viewModel
        /*PopupSizeConstants popupSizeConstants*/)
	{
		InitializeComponent();
        BindingContext = viewModel;
        Size = new(300, 500);
    }
}
