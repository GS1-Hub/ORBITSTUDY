using ORBITSTUDY.Helpers;

namespace ORBITSTUDY.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        imgCenter.Opacity = 0;
        imgCenter.TranslationY = 30;

        await Task.WhenAll(
            imgCenter.FadeToAsync(1, 1000, Easing.CubicOut),
            imgCenter.TranslateToAsync(0, 0, 1000, Easing.SinInOut));

        _ = StarFloatingAnimation();
    }

    private async Task StarFloatingAnimation()
    {
        while (true)
        {
            await imgCenter.TranslateToAsync(0, -12, 2000, Easing.SinInOut);
            await imgCenter.TranslateToAsync(0, 12, 2000, Easing.SinInOut);
        }
    }

    private async void btnSettings_Clicked(object sender, EventArgs e)
    {
		var btn = (ImageButton)sender;

		await btn.PlayPressAnimation();
    }

    private void btnStart_Clicked(object sender, EventArgs e)
    {

    }

    private void btnInfo_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAnchiments_Clicked(object sender, EventArgs e)
    {

    }
}