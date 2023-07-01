using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Yelp.Api;

namespace Diner.ViewModels
{
	public class FeedViewModel
	{
        public ReactiveCollection<Yelp.Api.Models.BusinessResponse> Businesses { get; } = new();
        public AsyncReactiveCommand RefreshCommand { get; } = new();
        public ReactiveProperty<bool> IsRefreshing { get; set; } = new();
        public AsyncReactiveCommand SelectNoteCommand { get; } = new();
        public ReactiveProperty<string> SearchTerm { get; set; } = new();
        public AsyncReactiveCommand AddToListCommand { get; set; } = new();

        public FeedViewModel()
		{
            RefreshCommand.Subscribe(async _ => await FindAsync());
            SelectNoteCommand.Subscribe(async business => await OpenBusiness(business));
            AddToListCommand.Subscribe(async business => await AddToList(business));
        }

        private async Task AddToList(object business)
        {
            //await DisplayAlert("Alert", "You have been alerted", "OK");
        }

        private async Task FindAsync()
        {
            IsRefreshing.Value = true;
            Businesses.Clear();
            var request = new Yelp.Api.Models.SearchRequest();
            request.Latitude = 34.0211;
            request.Longitude = -118.415016;
            request.Term = SearchTerm.Value;
            request.MaxResults = 15;

            var client = new Yelp.Api.Client("IrsBbxZg4DcGnOIluEU_-Qk9y6U2lt4__1rHcAK2fCM6MSbaWZNBAtJzhd8rTciuJ2Q5WbWbi2U29DKlQOr5GhfiDx8_yS2YN4xaRYh8vhN_MG8OVkhWfkFAhLWVZHYx");
            var results = await client.SearchBusinessesAllAsync(request);
            foreach (Yelp.Api.Models.BusinessResponse business in results.Businesses) {
                Businesses.Add(business);
            }
            IsRefreshing.Value = false;
        }

        private async Task OpenBusiness(object business)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "business", business }
            };
            await Shell.Current.GoToAsync($"{nameof(Views.BusinessPage)}", navigationParameter);
        }
    }
}

