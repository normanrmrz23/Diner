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
		public ReactiveCollection<Yelp.Api.Models.BusinessResponse> Businesses { get; set; }
    }
}

