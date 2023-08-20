using System;
namespace Diner.Models
{
	public class FeedItem
	{
		public string Source { get; set; }
		public string User { get; set; }
		public string Action { get; set; }
		public string Location { get; set; }
		public ImageSource Photo { get; set; }

		public FeedItem()
		{
		}
	}
}

