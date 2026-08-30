using ORBITSTUDY.Helpers;

namespace ORBITSTUDY.Pages;

public partial class HomePage : ContentPage
{
    private string? _level;
    public string? Level
    {
        get => _level;
        set
        {
            if (_level != value)
            {
                _level = value;
                OnPropertyChanged();
            }
        }
    }
    public HomePage()
	{
		InitializeComponent();

        BindingContext = this;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        GetPlayerStats();

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

    private async void btnStart_Clicked(object sender, EventArgs e)
    {
        await btnStart.ScaleToAsync(0.9, 50, Easing.CubicOut);
        await btnStart.ScaleToAsync(1.0, 50, Easing.CubicIn);

        await Task.WhenAll(
            imgCenter.ScaleToAsync(15, 500, Easing.CubicIn),
            this.FadeToAsync(0, 400, Easing.CubicOut));

        await Shell.Current.GoToAsync(nameof(FocusSessionPage));

        imgCenter.Scale = 1;
        this.Opacity = 1;
    }

    private void btnInfo_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAnchiments_Clicked(object sender, EventArgs e)
    {

    }

    private void GetPlayerStats()
    {
        Level = Preferences.Get("lvl", string.Empty);
    }
}