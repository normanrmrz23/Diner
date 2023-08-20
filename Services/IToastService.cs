using System;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Diner.Services
{
    public interface IToastService
    {
        Task<object> MakeToastAsync(string text);
    }

    public class ToastService : IToastService
    {
        public async Task<object> MakeToastAsync(string text)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
            return Task.CompletedTask;
        }

    }
}
