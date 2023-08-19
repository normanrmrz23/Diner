using System;
using CommunityToolkit.Maui.Views;

namespace Diner.Services
{
    public interface IPopupService
    {
        Task<object> ShowPopupAsync(Popup popup);
        void ClosePopup();
    }

    public class PopupService : IPopupService
    {
        public async Task<object> ShowPopupAsync(Popup popup)
        {
            Page page = Shell.Current.CurrentPage ?? throw new NullReferenceException();
            var result = await page.ShowPopupAsync(popup);
            return result;
        }

        public void ClosePopup()
        {
            Page page = Shell.Current.CurrentPage ?? throw new NullReferenceException();
            
        }
        
    }
}

