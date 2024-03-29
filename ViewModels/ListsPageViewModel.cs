﻿using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Diner.Framework;
using Diner.Services;

namespace Diner.ViewModels;

public class ListsPageViewModel : ObservableObject, IQueryAttributable
{
    private Models.Lists _list;
    private readonly IListLoader _listLoader;

    public string Text
    {
        get => _list.Text;
        set
        {
            if (_list.Text != value)
            {
                _list.Text = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime Date => _list.Date;

    public string Identifier => _list.Filename;

    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }

    public ListsPageViewModel(IListLoader listLoader)
    {
        _listLoader = listLoader;
        _listLoader.LoadAsync("MyList");
        _list = new Models.Lists();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public ListsPageViewModel(Models.Lists note)
    {
        _list = note;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    private async Task Save()
    {
        _list.Date = DateTime.Now;
        _list.Save();
        await Shell.Current.GoToAsync($"..?saved={_list.Filename}");
    }

    private async Task Delete()
    {
        _list.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_list.Filename}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _list = Models.Lists.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _list = Models.Lists.Load(_list.Filename);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Date));
    }
}