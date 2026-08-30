using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using ORBITSTUDY.Database;
using ORBITSTUDY.Helpers;
using ORBITSTUDY.Models;
using System.Net.Mail;

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
        if (string.IsNullOrEmpty(newPlayer.Username) ||
            string.IsNullOrEmpty(newPlayer.Password))
        {
            await ToastHelper.MakeToast(
                "Oops! Something is wrong!",
                ToastDuration.Short,
                14);

            return;
        }

        if (!IsValidEmail(newPlayer.Email))
        {
            await ToastHelper.MakeToast(
                $"Invalid email: {newPlayer.Email}",
                ToastDuration.Short,
                14);

            return;
        }

        await _db.InitializeDataBaseAsync();
        await _db.CreatePlayer(newPlayer);
        await _db.CreatePlayerStats(newPlayer.Id);

        var playerlvl = await _db.GetPlayerStats(newPlayer.Id);
        Preferences.Set("lvl", playerlvl?.LVL);

        IsRegistered = true;
        await CloseAsync();
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var validEmail = new MailAddress(email);
            return validEmail.Address == email;
        }
        catch
        {
            return false;
        }
    }
}