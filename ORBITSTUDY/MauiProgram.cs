using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ORBITSTUDY.Database;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace ORBITSTUDY
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            SQLitePCL.Batteries_V2.Init();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SpaceGrotesk-Bold", "SpaceGroteskBold");
                    fonts.AddFont("SpaceGrotesk-Light", "SpaceGroteskLight");
                    fonts.AddFont("SpaceGrotesk-Medium", "SpaceGroteskMedium");
                    fonts.AddFont("SpaceGrotesk-Regular", "SpaceGroteskRegular");
                    fonts.AddFont("SpaceGrotesk-SemiBold", "SpaceGroteskSemiBold");
                });

            builder.Services.AddSingleton<DataBaseService>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
