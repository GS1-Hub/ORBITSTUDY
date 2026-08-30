using ORBITSTUDY.Pages;

namespace ORBITSTUDY
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(FocusSessionPage), typeof(FocusSessionPage));
        }
    }
}
