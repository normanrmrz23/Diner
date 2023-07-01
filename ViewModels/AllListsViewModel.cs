using CommunityToolkit.Mvvm.Input;
using Diner.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Diner.ViewModels;

internal class AllListsViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.ListsViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    public AllListsViewModel()
    {
        AllNotes = new ObservableCollection<ViewModels.ListsViewModel>(Models.Lists.LoadAll().Select(n => new ListsViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<ViewModels.ListsViewModel>(SelectListAsync);
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.Lists));
    }

    private async Task SelectListAsync(ViewModels.ListsViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.Lists)}?load={note.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            ListsViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            ListsViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }

            // If note isn't found, it's new; add it.
            else
                AllNotes.Insert(0, new ListsViewModel(Models.Lists.Load(noteId)));
        }
    }
}

