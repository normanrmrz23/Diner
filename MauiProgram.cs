using Diner.Models;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Diner.Views;
using Diner.ViewModels;
using Diner.Services;
using Diner.Abstractions;

namespace Diner;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            fonts.AddFont("Alta_regular.otf", "AltaRegular");
            fonts.AddFont("Alta_light.otf", "AltaLight");
            fonts.AddFont("Alta_caption.otf", "AltaCaption");
            fonts.AddFont("calibri.ttf", "Calibri");
            fonts.AddFont("fa-regular.otf", "FontAwesome");
            fonts.AddFont("fa-solid-900.otf", "FontAwesomeSolid");
            fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
            fonts.AddFont("OtomanopeeOne-Regular.ttf", "OtomanopeeRegular");
        }).UseMauiCommunityToolkit();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<HomePageViewModel>();
        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddTransient<SearchPageViewModel>();
        builder.Services.AddTransient<AllListsPage>();
        builder.Services.AddTransient<AllListsPageViewModel>();
        builder.Services.AddSingleton<IAlertService, AlertService>();
        builder.Services.AddSingleton<Data>();
        builder.Services.AddScoped<IListWriter, ListWriter>();
        builder.Services.AddScoped<IListLoader, ListLoader>();
        builder.Services.AddScoped<IFileDb, FileDb>();
        builder.Services.AddScoped<ISystemIODirectory, SystemIODirectory>();
        builder.Services.AddScoped<ISystemIOFile, SystemIOFile>();
        builder.Services.AddScoped<IExternalStoragePath, ExternalStoragePath>();
        builder.Services.AddScoped<IAppFolderService, AppFolderService>();
        builder.Services.AddTransient<IPopupService, PopupService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}