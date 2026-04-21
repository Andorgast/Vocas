namespace presentation_layer.Models
{
    public class UserViewModel
    {
        public string Username { get; set; } = "";
        //private string Password = "";
        public List<AvailabilityViewModel> Available { get; private set; } = [];
        public decimal KD { get; private set; }
        public int TeamKills { get; private set; }
        public TimeSpan Playtime { get; private set; }
        public string FavoredFactions { get; private set; }
        public int UserId { get; private set; }

        public UserViewModel(int userId, string username, decimal kD, int teamkills, TimeSpan playtime, string factions)
        {
            UserId = userId;
            Username = username;
            KD = kD;
            TeamKills = teamkills;
            Playtime = playtime;
            FavoredFactions = factions;
        }
    }
}
