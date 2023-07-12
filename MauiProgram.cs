using Microsoft.Extensions.Logging;

namespace Diner;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
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
            });
        builder.Services.AddSingleton<IAlertService, AlertService>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

