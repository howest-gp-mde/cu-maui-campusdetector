using CommunityToolkit.Maui;
using Mde.CampusDetector.Core.Campuses.Services;
using Mde.CampusDetector.ViewModels;
using Mde.CampusDetector.Views;
using Microsoft.Extensions.Logging;

namespace Mde.CampusDetector
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddTransient<ICampusService, CampusService>();

            builder.Services.AddTransient<IGeolocation>((provider) => Geolocation.Default);

            return builder.Build();
        }
    }
}
