using System.Threading.Tasks;

namespace Diner.Framework
{
    public interface INavigatedAware
    {
        Task OnNavigatedToAsync(NavigationParameters parameters);
        Task OnNavigatedFromAsync();
        Task OnPagePoppedAsync();
    }
}
