using CommunityToolkit.Mvvm.Input;
using Diner.Framework;
using Diner.Models;
using Diner.Services;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Diner.ViewModels;

public class AllListsPageViewModel : ViewModelBase, IQueryAttributable
{
    private readonly IListLoader _listLoader;
    public List<string> _myList;
    public IEnumerable<string> _allLists;

    public ObservableCollection<ListsPageViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }
    public ReactiveCommand LoadListCommand { get; } = new();
    public AllListsPageViewModel(IListLoader listLoader)
    {
        _listLoader = listLoader;

        AllNotes = new ObservableCollection<ViewModels.ListsPageViewModel>(Models.Lists.LoadAll().Select(n => new ListsPageViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<ViewModels.ListsPageViewModel>(SelectListAsync);
        LoadListCommand.Subscribe(async _ => await LoadListsAsync());
    }

    private async Task LoadListsAsync()
    {
        _myList = await _listLoader.LoadAsync("MyList");
        _allLists = await _listLoader.LoadAllListsAsync();
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.ListsPage));
    }

    private async Task SelectListAsync(ViewModels.ListsPageViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.ListsPage)}?load={note.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            ListsPageViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            ListsPageViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }

            // If note isn't found, it's new; add it.
            else
                AllNotes.Insert(0, new ListsPageViewModel(Models.Lists.Load(noteId)));
        }
    }
}

