using Reactive.Bindings;
using Yelp.Api.Models;
using Diner.Models;
using Diner.Framework;
using Diner.Services;
using Diner.Views;
using Yelp.Api;
using System;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;

namespace Diner.ViewModels
{
    public class SearchPageViewModel : ViewModelBase
    {
        private readonly IListWriter _listWriter;
        private readonly IListLoader _listLoader;
        private readonly IPopupService _popupService;
        private AddToListPopupPage _popup;
        private CreateNewListPopupPage _newPopup;
        private readonly Client _client = new Client("IrsBbxZg4DcGnOIluEU_-Qk9y6U2lt4__1rHcAK2fCM6MSbaWZNBAtJzhd8rTciuJ2Q5WbWbi2U29DKlQOr5GhfiDx8_yS2YN4xaRYh8vhN_MG8OVkhWfkFAhLWVZHYx");

        Data database;
        public ReactiveCollection<BusinessResponse> Businesses { get; } = new();
        public AsyncReactiveCommand RefreshCommand { get; } = new();
        public ReactiveProperty<bool> IsRefreshing { get; set; } = new();
        public AsyncReactiveCommand SelectBusinessCommand { get; } = new();
        public ReactiveProperty<string> SearchTerm { get; set; } = new("Bakery");
        public AsyncReactiveCommand AddToListCommand { get; set; } = new();
        public Microsoft.Maui.Devices.Sensors.Location UserLocation { get; set; } = new();
        public BusinessList MyList { get; set; } = new();
        public List<BusinessList> MyLists { get; set; } = new();

        public SearchPageViewModel(IListWriter listWriter,
            IListLoader listLoader,
            IPopupService popupService)
		{
            _listWriter = listWriter;
            _listLoader = listLoader;
            _popupService = popupService;

            RefreshCommand.Subscribe(async _ => await FindAsync());
            SelectBusinessCommand.Subscribe(OpenBusiness);
            AddToListCommand.Subscribe(async business => await ShowAddToListPopup(business));
         //   SearchTerm.Subscribe(async _ => await AutoCompleteAsync());
            FindAsync();
        }

        private async Task AutoCompleteAsync()
        {
            await GetCurrentLocation();
            var response = await _client.AutocompleteAsync(SearchTerm.Value, UserLocation.Latitude, UserLocation.Longitude);
            AutoCompleteBusinessList(response);
        }

        private void AutoCompleteBusinessList(AutocompleteResponse response)
        {
            Businesses.Clear();
            foreach (BusinessResponse business in response.Businesses)
            {
                Businesses.Add(business);
            }
        }

        private async Task FindAsync()
        {
            IsRefreshing.Value = true;
            Businesses.Clear();

            var request = new SearchRequest();
            await GetCurrentLocation();
            request.Latitude = UserLocation.Latitude;
            request.Longitude = UserLocation.Longitude;
            request.Term = SearchTerm.Value;
            request.MaxResults = 40;
            request.SortBy = "distance";
            request.Categories = "restaurants";

            var results = await _client.SearchBusinessesAllAsync(request);

            UpdateBusinessList(results);
            IsRefreshing.Value = false;
        }

        private void UpdateBusinessList(SearchResponse results)
        {
            foreach (BusinessResponse business in results.Businesses)
            {
                Businesses.Add(business);
            }
        }

        private async Task OpenBusiness(object business)
        {
            if (business == null)
                return;
            var navigationParameter = new Dictionary<string, object>
            {
                { "business", business }
            };
            await Shell.Current.GoToAsync($"{nameof(Views.BusinessPage)}", navigationParameter);
        }


        private async Task ShowAddToListPopup(object business)
        {
            async void action(bool result)
            {
                _popup.Close(result);
                await Task.Delay(400);
                if (result)
                {
                    async void close()
                    {
                        _newPopup.Close();
                    }
                    _newPopup = new CreateNewListPopupPage(new CreateNewListPopupPageViewModel(business as BusinessResponse, _listWriter, _listLoader, _popupService, close));
                    await _popupService.ShowPopupAsync(_newPopup);
                }
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                string text = "Added to List";
                ToastDuration duration = ToastDuration.Short;
                double fontSize = 14;

                var toast = Toast.Make(text, duration, fontSize);

                await toast.Show(cancellationTokenSource.Token);
            }
            _popup = new AddToListPopupPage(new AddToListPopupPageViewModel(business as BusinessResponse, _listWriter, _listLoader, _popupService, action));
            var result = await _popupService.ShowPopupAsync(_popup);
        }

        public async Task<string> GetCachedLocation()
        {
            try
            {
                Microsoft.Maui.Devices.Sensors.Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location != null)
                    return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                //
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return "None";
        }

        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public async Task GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                UserLocation = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (UserLocation != null)
                    Console.WriteLine($"Latitude: {UserLocation.Latitude}, Longitude: {UserLocation.Longitude}, Altitude: {UserLocation.Altitude}");
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
    }
}

