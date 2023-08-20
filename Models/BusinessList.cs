using System;
using Reactive.Bindings;
using SQLite;

namespace Diner.Models
{
	public class BusinessList
	{
		public BusinessList()
		{
		}

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
		public string ListName { get; set; }
		public List<Yelp.Api.Models.BusinessResponse> Businesses { get; set; }
    }
}

