using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using ORBITSTUDY.Database;
using ORBITSTUDY.Helpers;
using ORBITSTUDY.Models;

namespace ORBITSTUDY.Pages;

public partial class Login : ContentPage
{
    private readonly DataBaseService _dbService;
    public Login()
	{
		InitializeComponent();
        _dbService = new DataBaseService();
	}

    private async void btnSignin_Clicked(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(txt_username.Text) || string.IsNullOrEmpty(txt_password.Text))
        {
            await ToastHelper.MakeToast("Oops! Something is missing", ToastDuration.Short, 14);
            return;
        }

        bool isValid = await _dbService.Login(txt_username.Text, txt_password.Text);

        if (isValid)
        {
            await Shell.Current.GoToAsync(nameof(LoadingPage));
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

        await this.ShowPopupAsync(popup);

        if (popup.IsRegistered)
            await ToastHelper.MakeToast("Great! You are a player now", ToastDuration.Long, 14);
    }
}