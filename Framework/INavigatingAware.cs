namespace Diner.Framework
{
    public interface INavigatingAware
    {
        Task OnNavigatingToAsync(NavigationParameters parameters);
    }
}

