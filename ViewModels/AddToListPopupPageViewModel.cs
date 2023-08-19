using System;
using System.Collections.ObjectModel;
using Diner.Framework;
using Diner.Services;
using Reactive.Bindings;
using Yelp.Api.Models;

namespace Diner.ViewModels
{
	public class AddToListPopupPageViewModel : ViewModelBase
	{
        private readonly IListWriter _listWriter;
        private readonly IListLoader _listLoader;
        private Action _closeCommand;
        public BusinessResponse Business { get; set; } = new();
        public ObservableCollection<string> Lists { get; } = new();
        public AsyncReactiveCommand SelectListCommand { get; } = new();

        public AddToListPopupPageViewModel(BusinessResponse business,
        IListWriter listWriter,
        IListLoader listLoader,
        Action closeCommand)
		{
            _listWriter = listWriter;
            _listLoader = listLoader;
            _closeCommand = closeCommand;

            Business = business;

            Lists = new ObservableCollection<string>(_listLoader.LoadAllListsAsync().Result);
            SelectListCommand.Subscribe(AddToSpecifiedList);
        }

        private async Task AddToSpecifiedList(object listName)
		{
            var b = Business;
            //MyList.Businesses.Add(b);
            await _listWriter.WriteAsync(listName as string, b);
            CloseCommand();
        }

        private void CloseCommand()
        {
            _closeCommand();
        }

        private Task SaveToSpecifiedList(object business)
        {
            throw new NotImplementedException();
        }

        private Task SaveToNewList(object business)
        {
            throw new NotImplementedException();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            //await _listWriter.WriteAsync("MyList", Business);
        }

    }
}

