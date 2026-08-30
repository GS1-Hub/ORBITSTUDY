using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace ORBITSTUDY.Pages;

public partial class FocusSessionPage : ContentPage
{
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

    public FocusSessionPage()
    {
        InitializeComponent();
        InitStars();
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
        _animationTimer.Interval = TimeSpan.FromMilliseconds(30); // ~33 FPS
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

        _focusTimer = Dispatcher.CreateTimer();
        _focusTimer.Interval = TimeSpan.FromSeconds(1);
        _focusTimer.Tick += (s, e) =>
        {
            if (_isSessionActive)
            {
                _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
                LblTimer.Text = _elapsedTime.ToString(@"hh\:mm\:ss");
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

        await DisplayAlertAsync("Viagem Concluída!", $"Parabéns! Focou-se durante {_elapsedTime.ToString(@"mm")} minutos e ganhou {totalXpEarned} XP.", "Fantástico!");
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
}