using Reactive.Bindings;
using Yelp.Api.Models;
using Diner.Models;
using Diner.Framework;
using Diner.Services;

namespace Diner.ViewModels
{
    public class SearchPageViewModel : ViewModelBase
    {
        private readonly IListWriter _listWriter;

        Data database;
        public ReactiveCollection<BusinessResponse> Businesses { get; } = new();
        public AsyncReactiveCommand RefreshCommand { get; } = new();
        public ReactiveProperty<bool> IsRefreshing { get; set; } = new();
        public AsyncReactiveCommand SelectBusinessCommand { get; } = new();
        public ReactiveProperty<string> SearchTerm { get; set; } = new();
        public AsyncReactiveCommand AddToListCommand { get; set; } = new();
        public Microsoft.Maui.Devices.Sensors.Location UserLocation { get; set; } = new();
        public BusinessList MyList { get; set; } = new();
        public List<BusinessList> MyLists { get; set; } = new();

        public SearchPageViewModel(IListWriter listWriter)
		{
            _listWriter = listWriter;

            RefreshCommand.Subscribe(async _ => FindAsync());
            SelectBusinessCommand.Subscribe(async business => await OpenBusiness(business));
            AddToListCommand.Subscribe(async business => await AddToList(business));
        }

        private async Task AddToList(object business)
        {
            var b = business as BusinessResponse;
            var message = b.Name + " has successfully been added to MyList";
            await App.AlertSvc.ShowAlertAsync("", message);
            //MyList.Businesses.Add(b);
            await _listWriter.WriteAsync("MyList", b);
        }

        private async void FindAsync()
        {
            IsRefreshing.Value = true;
            Businesses.Clear();
            var request = new SearchRequest();
            await GetCurrentLocation();
            request.Latitude = UserLocation.Latitude;
            request.Longitude = UserLocation.Longitude;
            request.Term = SearchTerm.Value;
            request.MaxResults = 20;
            request.SortBy = "rating";
            //filter out non restaurants
            var client = new Yelp.Api.Client("IrsBbxZg4DcGnOIluEU_-Qk9y6U2lt4__1rHcAK2fCM6MSbaWZNBAtJzhd8rTciuJ2Q5WbWbi2U29DKlQOr5GhfiDx8_yS2YN4xaRYh8vhN_MG8OVkhWfkFAhLWVZHYx");
            var results = await client.SearchBusinessesAllAsync(request);
            foreach (BusinessResponse business in results.Businesses) {
                Businesses.Add(business);
            }
            IsRefreshing.Value = false;
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

