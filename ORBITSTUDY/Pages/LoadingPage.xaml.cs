namespace ORBITSTUDY.Pages;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadingProgressBar.ProgressTo(1, 3000, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(HomePage));
    }
}