using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ORBITSTUDY.Helpers
{
    public static class ToastHelper
    {
        public static async Task MakeToast(string text, ToastDuration duration = ToastDuration.Short, double size = 14)
        {
            var toast = Toast.Make(text, duration, size);
            await toast.Show();
        }
    }
}