using Microsoft.Maui.Controls;
using System.Runtime.CompilerServices;

namespace ORBITSTUDY.Helpers
{
    public static class EffectsHelper
    {
        public static async Task PlayPressAnimation(this View view)
        {
            var scaleTask = view.ScaleToAsync(0.95, 80, Easing.CubicOut);
            var fadeTask = view.FadeToAsync(0.85, 80, Easing.CubicOut);
            await Task.WhenAll(scaleTask, fadeTask);

            var scaleBack = view.ScaleToAsync(1.0, 100, Easing.CubicIn);
            var fadeBack = view.FadeToAsync(1.0, 100, Easing.CubicIn);
            await Task.WhenAll(scaleBack, fadeBack);
        }
    }
}
