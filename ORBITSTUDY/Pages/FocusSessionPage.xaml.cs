using ORBITSTUDY.Database;
using ORBITSTUDY.Models;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.ComponentModel;

namespace ORBITSTUDY.Pages;

public partial class FocusSessionPage : ContentPage, INotifyPropertyChanged
{
    private string? _currentPlanetIcon;

    public string? CurrentPlanetIcon
    {
        get => _currentPlanetIcon;
        set { _currentPlanetIcon = value; OnPropertyChanged(); }
    }
    private class Star
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Size { get; set; }
        public float Speed { get; set; }
    }

    private List<Star> _stars = new();
    private IDispatcherTimer? _animationTimer;
    private IDispatcherTimer? _focusTimer;
    private TimeSpan _elapsedTime;
    private bool _isSessionActive;
    private Random _random = new();
    private DataBaseService _service = new();
    private int playerId = Preferences.Get("player_id", 0);

    public FocusSessionPage()
    {
        InitializeComponent();

        BindingContext = this;

        InitStars();
        LoadCurrentPlanet();
    }

    private void LoadCurrentPlanet()
    {

        string playerLvl = Preferences.Get("lvl", "NOOB");
        var planet = PlanetData.GameProgress.Planets.FirstOrDefault(p => p.LvlRequired.Equals(playerLvl, StringComparison.OrdinalIgnoreCase));
        try
        {
            if (planet != null)
            {
                CurrentPlanetIcon = planet.Icon;
            }
            else
            {
                CurrentPlanetIcon = PlanetData.GameProgress.Planets.First().Icon;
            }
        }
        catch
        {
            CurrentPlanetIcon = "mercury_icon.png";
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        StartSpaceFlightAnimation();
        StartFocusSession();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _animationTimer?.Stop();
        _focusTimer?.Stop();
    }

    private void InitStars()
    {
        for (int i = 0; i < 60; i++)
        {
            _stars.Add(new Star
            {
                X = (float)_random.NextDouble() * 1000,
                Y = (float)_random.NextDouble() * 2000,
                Size = (float)_random.NextDouble() * 3 + 1,
                Speed = (float)_random.NextDouble() * 12 + 4
            });
        }
    }

    private void StartSpaceFlightAnimation()
    {
        _animationTimer = Dispatcher.CreateTimer();
        _animationTimer.Interval = TimeSpan.FromMilliseconds(30);
        _animationTimer.Tick += (s, e) =>
        {
            foreach (var star in _stars)
            {
                star.Y += star.Speed;
                if (star.Y > 2000)
                {
                    star.Y = 0;
                    star.X = (float)_random.NextDouble() * 1000;
                }
            }
            CanvasView.InvalidateSurface();
        };
        _animationTimer.Start();
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColor.Parse("#080B14"));

        using var paint = new SKPaint
        {
            IsAntialias = true
        };

        foreach (var star in _stars)
        {
            paint.Color = SKColors.White.WithAlpha((byte)_random.Next(120, 255));
            canvas.DrawCircle(star.X, star.Y, star.Size, paint);
        }
    }

    private void StartFocusSession()
    {
        _elapsedTime = TimeSpan.Zero;
        _isSessionActive = true;

        UpdatePlanetByTime();

        _focusTimer = Dispatcher.CreateTimer();
        _focusTimer.Interval = TimeSpan.FromSeconds(1);

        _focusTimer.Tick += (s, e) =>
        {
            if (_isSessionActive)
            {
                _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));

                LblTimer.Text = _elapsedTime.ToString(@"hh\:mm\:ss");

                UpdatePlanetByTime();
            }
        };

        _focusTimer.Start();
    }


    private async void BtnFinish_Clicked(object sender, EventArgs e)
    {
        _isSessionActive = false;
        _focusTimer?.Stop();
        _animationTimer?.Stop();

        int totalXpEarned = (int)(_elapsedTime.TotalMinutes * 10);

        PlayerStats? playerStats = await _service.GetPlayerStats(playerId);

        if (playerStats == null)
        {
            await DisplayAlertAsync("Erro","Não foi possível encontrar os dados do jogador.","OK");

            return;
        }

        playerStats.XP += totalXpEarned;
        Preferences.Set("xp", playerStats.XP);

        playerStats.LVL = CalculateLevel(playerStats.XP);

        bool success = await _service.UpdatePlayerStats(playerStats);

        if (!success)
        {
            await DisplayAlertAsync(
                "Erro",
                "Não foi possível guardar o progresso.",
                "OK");

            return;
        }

        await DisplayAlertAsync(
            "Viagem Concluída!",
            $"Parabéns! Focou-se durante {_elapsedTime:mm\\:ss} " +
            $"e ganhou {totalXpEarned} XP!\n\n" +
            $"XP total: {playerStats.XP}\n" +
            $"Nível: {playerStats.LVL}",
            "Fantástico!");

        await Shell.Current.GoToAsync(nameof(HomePage));
    }

    private async void BtnAbort_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync("Abortar Viagem?", "Se abortar agora, perderá o progresso desta sessão de foco.", "Sim, sair", "Continuar focado");

        if (confirm)
        {
            _isSessionActive = false;
            _focusTimer?.Stop();
            _animationTimer?.Stop();
            await Shell.Current.GoToAsync(nameof(HomePage));
        }
    }

    private async void BtnPause_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync("Pause Trip?", "Its is going to stop the timer.", "Yes!", "No!");

        if (confirm)
        {
            _focusTimer?.Stop();
            _animationTimer?.Stop();
            BtnPause.IsVisible = false;
            BtnContinue.IsVisible = true;
        }
    }
    private async void BtnContinue_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync("Continue Trip?", "Its is going to continue the trip.", "Yes!", "No!");

        if (confirm)
        {
            _focusTimer?.Start();
            _animationTimer?.Start();
            BtnContinue.IsVisible = false;
            BtnPause.IsVisible = true;
        }
    }

    private string CalculateLevel(int xp)
    {
        if (xp >= 1000)
            return "MASTER";

        if (xp >= 500)
            return "PRO";

        if (xp >= 250)
            return "ADVANCED";

        if (xp >= 100)
            return "STUDENT";

        return "NOOB";
    }
    private void UpdatePlanetByTime()
    {
        int planetIndex = (int)(_elapsedTime.TotalMinutes / 10);

        if (planetIndex >= PlanetData.GameProgress.Planets.Count)
        {
            planetIndex = PlanetData.GameProgress.Planets.Count - 1;
        }

        var planet = PlanetData.GameProgress.Planets[planetIndex];

        if (CurrentPlanetIcon != planet.Icon)
        {
            CurrentPlanetIcon = planet.Icon;

            PlanetData.GameProgress.CurrentPlanet = planetIndex;
        }
    }
}