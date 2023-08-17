using System;
using Diner.Models;
using Diner.Services;
using Reactive.Bindings;

namespace Diner.ViewModels
{
    public class HomePageViewModel
    {
        public HomePageViewModel(
            IAppFolderService appFolderService
        )
        {
            appFolderService.EnsureStructure();
           // AuthCommand.Subscribe(async _ => await AuthenticateWithAppleAsync());
        }
        public AsyncReactiveCommand AuthCommand { get; set; } = new();

  /*      public async Task AuthenticateWithAppleAsync()
        {

                // Use Native Apple Sign In API's
            var result = await AppleSignInAuthenticator.AuthenticateAsync();


            var authToken = string.Empty;

            if (result.Properties.TryGetValue("name", out string name) && !string.IsNullOrEmpty(name))
                authToken += $"Name: {name}{Environment.NewLine}";

            if (result.Properties.TryGetValue("email", out string email) && !string.IsNullOrEmpty(email))
                authToken += $"Email: {email}{Environment.NewLine}";

            // Note that Apple Sign In has an IdToken and not an AccessToken
            authToken += result?.AccessToken ?? result?.IdToken;
        }*/
    }
}

