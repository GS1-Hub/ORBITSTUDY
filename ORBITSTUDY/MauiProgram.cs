using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace ORBITSTUDY
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseSkiaSharp()
                .ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                    events.AddAndroid(android => android.OnCreate((activity, bundle) =>
                    {
                        activity?.Window?.SetFlags(
                            Android.Views.WindowManagerFlags.LayoutNoLimits,
                            Android.Views.WindowManagerFlags.LayoutNoLimits);
                    }));
#endif
                })
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
