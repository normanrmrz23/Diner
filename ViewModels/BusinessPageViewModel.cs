using System;
using Reactive.Bindings;
using static System.Net.Mime.MediaTypeNames;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Diner.Models;

namespace Diner.ViewModels
{
	public partial class BusinessPageViewModel : ObservableObject, IQueryAttributable
    {
        private Models.Business _business;

        public BusinessPageViewModel()
		{
            OpenWebsiteCommand.Subscribe(async _ => await OpenWebsiteAsync());
            // _business = new Models.Business();
        }

        public BusinessPageViewModel(Models.Business business)
        {
            _business = business;
        }

        public ReactiveProperty<string> Name { get; set; } = new();
        public ReactiveProperty<string> Rating { get; set; } = new();
        public ReactiveProperty<string> Price { get; set; } = new();
        public ReactiveProperty<string> Phone { get; set; } = new();
        public ReactiveProperty<string> ImageUrl { get; set; } = new();
        public ReactiveProperty<float> Distance { get; set; } = new();
        public ReactiveProperty<double> DistanceAway { get; set; } = new();
        public ReactiveProperty<Yelp.Api.Models.Hour[]> Hours { get; set; } = new();
        public ReactiveProperty<string> Photos { get; set; } = new();
        public ReactiveProperty<bool> IsClosed { get; set; } = new();
        public ReactiveProperty<int> ReviewCount { get; set; } = new();
        public ReactiveProperty<string> Url { get; set; } = new();

        public Yelp.Api.Models.Category[] Categories { get; set; }
        public Yelp.Api.Models.Coordinates Coordinates { get; set; }
        public string DisplayPhone { get; set; }

        public string Id { get; set; }
        public bool IsClaimed { get; set; }
        public Yelp.Api.Models.Location Location { get; set; }

        public AsyncReactiveCommand OpenWebsiteCommand { get; set; } = new();

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            
            var b = query["business"] as Yelp.Api.Models.BusinessResponse;
            _business = Business.Load(b);
            RefreshProperties();
        }


        private async Task OpenWebsiteAsync()
        {
            await Launcher.Default.OpenAsync(Url.Value);
        }

        private void RefreshProperties()
        {
            Name.Value = _business.Name;
            ImageUrl.Value = _business.ImageUrl;
            Phone.Value = _business.Phone;
            Rating.Value = _business.Rating.ToString();
            ReviewCount.Value = _business.ReviewCount;
            Photos.Value = _business.Photos;
            Distance.Value = _business.Distance;
            DistanceAway.Value = _business.DistanceAway;
            Hours.Value = _business.Hours;
            IsClosed.Value = _business.IsClosed;
            Price.Value = _business.Price;
            Url.Value = _business.Url;
        }

    }
}

