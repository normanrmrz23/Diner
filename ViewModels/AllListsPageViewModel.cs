using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using Diner.Framework;
using Diner.Models;
using Diner.Services;
using Diner.Views;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Yelp.Api;
using Yelp.Api.Models;

namespace Diner.ViewModels;

public class AllListsPageViewModel : ViewModelBase
{
    private readonly IListLoader _listLoader;
    private readonly IListWriter _listWriter;
    private readonly IPopupService _popupService;

    private readonly Client _client = new Client("IrsBbxZg4DcGnOIluEU_-Qk9y6U2lt4__1rHcAK2fCM6MSbaWZNBAtJzhd8rTciuJ2Q5WbWbi2U29DKlQOr5GhfiDx8_yS2YN4xaRYh8vhN_MG8OVkhWfkFAhLWVZHYx");

    public List<string> _myList;
    public IEnumerable<string> _allLists;

    public IEnumerable<string> AllLists { get; set; }
    public ObservableCollection<ListsPageViewModel> AllNotes { get; }
    public ReactiveCollection<BusinessResponse> Businesses { get; } = new();
    public ReactiveCollection<BusinessList> BusinessLists { get; } = new();

    public ReactiveCommand NewCommand { get; } = new();
    public ReactiveCommand SelectListCommand { get; } = new();
    public ReactiveCommand LoadListsCommand { get; } = new();
    public ReactiveCommand DeleteListCommand { get; } = new();

    public AllListsPageViewModel(
        IListLoader listLoader,
        IListWriter listWriter,
        IPopupService popupService
        )
    {
        _listLoader = listLoader;
        _listWriter = listWriter;
        _popupService = popupService;

        AllNotes = new ObservableCollection<ViewModels.ListsPageViewModel>(Models.Lists.LoadAll().Select(n => new ListsPageViewModel(n)));
        NewCommand.Subscribe();
        SelectListCommand.Subscribe(async _ => await SelectListAsync(_));
        LoadListsCommand.Subscribe(async _ => await LoadListsAsync());
        DeleteListCommand.Subscribe(DeleteList);
        LoadListsAsync();
    }

    private async Task LoadListsAsync()
    {
        AllLists = await _listLoader.LoadAllListsAsync();
        foreach(string list in AllLists)
        {
            var bl = new BusinessList();
            bl.Businesses = new();
            var listData = await _listLoader.LoadAsync(list);
            foreach (string id in listData)
            {
                var business = await _client.GetBusinessAsync(id);
                bl.Businesses.Add(business);
            }
            bl.ListName = list;
            BusinessLists.Add(bl);
        }
    }

    private void DeleteList(object list)
    {
        _listWriter.DeleteExisting(list as string);
    }
    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.ListsPage));
    }

    private async Task SelectListAsync(object list)
    {
        AllLists = await _listLoader.LoadAllListsAsync(); //temp fix to reload lists when we come back to this page
        Businesses.Clear();
        var listData = await _listLoader.LoadAsync(list as string);
        foreach(string id in listData)
        {
            var business = await _client.GetBusinessAsync(id);
            Businesses.Add(business);
        }
        await _popupService.ShowPopupAsync(new ShowListPopupPage(new ShowListPopupPageViewModel(list as BusinessList)));
    }
    private async void PushModal()
    {
        if (Application.Current?.MainPage is not null)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ContentPage());
        }
    }
}

