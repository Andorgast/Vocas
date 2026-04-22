namespace presentation_layer.Models
{
    public class UserViewModel
    {
        public int UserId { get; private set; }
        public string Username { get; set; } = "";
        public List<AvailabilityViewModel> Availability { get; private set; }
        public decimal? KD { get; private set; }
        public int? TeamKills { get; private set; }
        public TimeSpan? Playtime { get; private set; }
        public string? FavoredFactions { get; private set; }

        public UserViewModel(int userId, string username, List<AvailabilityViewModel> availability, decimal? kD, int? teamkills, TimeSpan? playtime, string? factions)
        {
            UserId = userId;
            Username = username;
            Availability = availability;
            KD = kD;
            TeamKills = teamkills;
            Playtime = playtime;
            FavoredFactions = factions;
        }
    }
}
