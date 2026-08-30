using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using ORBITSTUDY.Database;
using ORBITSTUDY.Helpers;

namespace ORBITSTUDY.Pages;

public partial class Login : ContentPage
{
    private readonly DataBaseService _dbService;
    public Login()
    {
        InitializeComponent();
        _dbService = new DataBaseService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var savedUsername = Preferences.Get("username", string.Empty);
        var savedPassword = Preferences.Get("password", string.Empty);

        if (savedUsername.Length > 0 && savedPassword.Length > 0)
        {
            bool AutoLogin = await _dbService.Login(savedUsername, savedPassword);
            if (AutoLogin)
            {
                await Shell.Current.GoToAsync(nameof(LoadingPage));
            }
            else
            {
                return;
            }
        }
    }

    private async void btnSignin_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txt_username.Text) || string.IsNullOrEmpty(txt_password.Text))
        {
            await ToastHelper.MakeToast("Oops! Something is missing", ToastDuration.Short, 14);
            return;
        }

        bool isValid = await _dbService.Login(txt_username.Text, txt_password.Text);

        if (isValid)
        {
            await Shell.Current.GoToAsync(nameof(LoadingPage));
            Preferences.Set("username", txt_username.Text);
            Preferences.Set("password", txt_password.Text);
        }
        else
        {
            await ToastHelper.MakeToast("Oops! Something is wrong!", ToastDuration.Long, 14);
            return;
        }
    }

    private async void btnRegister_Clicked(object sender, EventArgs e)
    {
        var popup = new RegisterPlayer();

        await this.ShowPopupAsync(popup, new PopupOptions
        {
            Shape = null,
            Shadow = null,
            PageOverlayColor = Color.FromArgb("#080B14")
        });

        if (popup.IsRegistered)
            await ToastHelper.MakeToast("Great! You are a player now", ToastDuration.Long, 14);
    }
}