using System;
using Diner.Models;

namespace Diner.ViewModels
{
	public class ShowListPopupPageViewModel
	{
		public BusinessList BusinessList { get; set; } = new();
		public ShowListPopupPageViewModel(
			BusinessList list)
		{
			BusinessList = list;
		}
	}
}

