using System.ComponentModel;

public interface IBusiness : INotifyPropertyChanged
{
    Yelp.Api.Models.Category[] Categories { get; set; }
    Yelp.Api.Models.Coordinates Coordinates { get; set; }
    string DisplayPhone { get; set; }
    float Distance { get; set; }
    double DistanceAway { get; set; }
    Yelp.Api.Models.Hour[] Hours { get; set; }
    string Id { get; set; }
    string ImageUrl { get; set; }
    bool IsClaimed { get; set; }
    bool IsClosed { get; set; }
    Yelp.Api.Models.Location Location { get; set; }
    string Name { get; set; }
    string Phone { get; set; }
    string Photos { get; set; }
    string Price { get; set; }
    float Rating { get; set; }
    int ReviewCount { get; set; }
    string Url { get; set; }
}

