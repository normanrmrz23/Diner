using System;
using SQLite;

namespace Diner.Models
{
	public class Business : Yelp.Api.Models.ResponseBase
	{
		public Business()
		{

		}
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public Yelp.Api.Models.Category[] Categories { get; set; }
        public Yelp.Api.Models.Coordinates Coordinates { get; set; }
        public string DisplayPhone { get; set; }
        public float Distance { get; set; }
        public double DistanceAway { get; set; }
        public Yelp.Api.Models.Hour[] Hours { get; set; }
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsClaimed { get; set; }
        public bool IsClosed { get; set; }
        public Yelp.Api.Models.Location Location { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Photos { get; set; }
        public string Price { get; set; }
        public float Rating { get; set; }
        public int ReviewCount { get; set; }
        public string Url { get; set; }

        public static Business Load(Yelp.Api.Models.BusinessResponse businessResponse)
        {
            return
                new()
                {
                    Categories = businessResponse.Categories,
                    Coordinates = businessResponse.Coordinates,
                    DisplayPhone = businessResponse.DisplayPhone,
                    Distance = businessResponse.Distance,
                    DistanceAway = businessResponse.DistanceAway,
                    Hours = businessResponse.Hours,
                    Id = businessResponse.Id,
                    ImageUrl = businessResponse.ImageUrl,
                    IsClaimed = businessResponse.IsClaimed,
                    IsClosed = businessResponse.IsClosed,
                    Location = businessResponse.Location,
                    Name = businessResponse.Name,
                    Phone = businessResponse.Phone,
                    Price = businessResponse.Price,
                    Rating = businessResponse.Rating,
                    ReviewCount = businessResponse.ReviewCount,
                    Url = businessResponse.Url
                };
        }
    }
}

