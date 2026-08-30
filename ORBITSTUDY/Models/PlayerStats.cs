namespace ORBITSTUDY.Models
{
    public class PlayerStats
    {
        public int Id { get; set; }
        public int Player_id { get; set; }
        public int XP { get; set; }
        public string LVL { get; set; } = string.Empty;
    }
}
