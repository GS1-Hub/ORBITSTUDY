namespace ORBITSTUDY.Models
{
    public class PlanetData
    {
        public string Icon { get; set; } = string.Empty;
        public string LvlRequired { get; set; } = string.Empty;

        public static class GameProgress
        {
            public static readonly List<PlanetData> Planets = new()
            {
                new PlanetData { Icon = "mercury_icon.png", LvlRequired = "NOOB" },
                new PlanetData { Icon = "venus_icon.png", LvlRequired = "NOOB 2" },
                new PlanetData { Icon = "earth_icon.png", LvlRequired = "STUDENT" },
                new PlanetData { Icon = "mars_icon.png", LvlRequired = "ADVANCED" },
                new PlanetData { Icon = "jupiter_icon.png", LvlRequired = "PRO" },
                new PlanetData { Icon = "saturn_icon.png", LvlRequired = "PRO 2" },
                new PlanetData { Icon = "uranus_icon.png", LvlRequired = "MASTER" },
                new PlanetData { Icon = "neptune_icon.png", LvlRequired = "MASTER 2" }
            };

            public static int CurrentPlanet
            {
                get => Preferences.Get("CurrentPlanet", 0);
                set => Preferences.Set("CurrentPlanet", value);
            }
        }
    }
}
