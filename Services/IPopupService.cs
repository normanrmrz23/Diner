using System;
using CommunityToolkit.Maui.Views;

namespace Diner.Services
{
    public interface IPopupService
    {
        void ShowPopup(Popup popup);
        void ClosePopup();
    }

    public class PopupService : IPopupService
    {
        public void ShowPopup(Popup popup)
        {
            Page page = Shell.Current.CurrentPage ?? throw new NullReferenceException();
            page.ShowPopup(popup);
        }

        public void ClosePopup()
        {
            Page page = Shell.Current.CurrentPage ?? throw new NullReferenceException();
            
        }
        
    }
}

