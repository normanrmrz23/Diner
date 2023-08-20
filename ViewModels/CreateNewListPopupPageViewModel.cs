using System;
using System.Collections.ObjectModel;
using Diner.Services;
using Diner.Views;
using Reactive.Bindings;
using Yelp.Api.Models;

namespace Diner.ViewModels
{
	public class CreateNewListPopupPageViewModel
	{
        private readonly IListWriter _listWriter;
        private readonly IListLoader _listLoader;
        private readonly IPopupService _popupService;

        private Action _closeCommand;
        private CreateNewListPopupPage _popup;

        public ReactiveProperty<string> ListName { get; set; } = new();
        public BusinessResponse Business { get; set; } = new();
        public ObservableCollection<string> Lists { get; } = new();
        public AsyncReactiveCommand CreateNewListCommand { get; } = new();
        public AsyncReactiveCommand CancelCommand { get; } = new();

        public CreateNewListPopupPageViewModel(
        BusinessResponse business,
        IListWriter listWriter,
        IListLoader listLoader,
        IPopupService popupService,
        Action closeCommand
        )
		{
            _listWriter = listWriter;
            _listLoader = listLoader;
            _popupService = popupService;
            _closeCommand = closeCommand;

            Business = business;

            CancelCommand.Subscribe(OnCancel);
            CreateNewListCommand.Subscribe(SaveAndCreateNewList);
        }

        private async Task OnCancel()
        {
            //_closeCommand(false);
        }

        private async Task SaveAndCreateNewList()
        {
            await _listWriter.WriteAsync(ListName.Value, Business);
            _closeCommand();
        }
    }
}

