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
                new PlanetData {Icon = "mercury_icon.png", LvlRequired = "NOOB"},
                new PlanetData {Icon = "venus_icon.png", LvlRequired = "NOOB 2"}
            };

            public static int CurrentPlanet
            {
                get => Preferences.Get("CurrentPlanet", 0);
                set => Preferences.Set("CurrentPlanet", value);
            }
        }
    }


}
