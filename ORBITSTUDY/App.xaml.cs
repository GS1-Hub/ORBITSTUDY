using Microsoft.Extensions.DependencyInjection;
using ORBITSTUDY.Database;

namespace ORBITSTUDY
{
    public partial class App : Application
    {
        public App(DataBaseService db)
        {
            InitializeComponent();
            _ = db.InitializeDataBaseAsync();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}