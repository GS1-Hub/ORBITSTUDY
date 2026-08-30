using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using ORBITSTUDY.Database;
using ORBITSTUDY.Helpers;
using ORBITSTUDY.Models;

namespace ORBITSTUDY.Pages;

public partial class RegisterPlayer : Popup
{
    private Player newPlayer { get; set; } = new();
    private readonly DataBaseService _db;
    public bool IsRegistered { get; private set; } = false;
    public RegisterPlayer()
    {
        InitializeComponent();
        _db = new DataBaseService();

        BindingContext = newPlayer;
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(newPlayer.Username) || string.IsNullOrEmpty(newPlayer.Password) || string.IsNullOrEmpty(newPlayer.Email))
        {
            await ToastHelper.MakeToast("Oops! Something is wrong!", ToastDuration.Short, 14);
            return;
        }

        await _db.InitializeDataBaseAsync();
        await _db.CreatePlayer(newPlayer);
        IsRegistered = true;
        await CloseAsync();
    }
}