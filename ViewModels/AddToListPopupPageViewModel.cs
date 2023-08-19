using System;
using System.Collections.ObjectModel;
using Diner.Framework;
using Diner.Services;
using Diner.Views;
using Reactive.Bindings;
using Yelp.Api.Models;

namespace Diner.ViewModels
{
	public class AddToListPopupPageViewModel : ViewModelBase
	{
        private readonly IListWriter _listWriter;
        private readonly IListLoader _listLoader;
        private readonly IPopupService _popupService;

        private Action<bool> _closeCommand;
        private CreateNewListPopupPage _popup;

        public BusinessResponse Business { get; set; } = new();
        public ObservableCollection<string> Lists { get; } = new();
        public AsyncReactiveCommand SelectListCommand { get; } = new();
        public AsyncReactiveCommand NewListCommand { get; } = new();

        public AddToListPopupPageViewModel(BusinessResponse business,
        IListWriter listWriter,
        IListLoader listLoader,
        IPopupService popupService,
        Action<bool> closeCommand)
		{
            _listWriter = listWriter;
            _listLoader = listLoader;
            _popupService = popupService;
            _closeCommand = closeCommand;

            Business = business;

            Lists = new ObservableCollection<string>(_listLoader.LoadAllListsAsync().Result);
            SelectListCommand.Subscribe(AddToSpecifiedList);
            NewListCommand.Subscribe(SaveToNewList);
        }

        private async Task AddToSpecifiedList(object listName)
		{
            var b = Business;
            //MyList.Businesses.Add(b);
            await _listWriter.WriteAsync(listName as string, b);
            CloseCommand(true);
        }

        private void CloseCommand(bool result)
        {
            _closeCommand(result);
        }

        private Task SaveToSpecifiedList(object business)
        {
            throw new NotImplementedException();
        }

        private async Task SaveToNewList(object business)
        {
            void action()
            {
                _popup.Close();
            }
            CloseCommand(true);
        }

    }
}

