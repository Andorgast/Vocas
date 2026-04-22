namespace data_layer
{
    public record UserDTO
    {
        public int? UserId { get; set; }
        public string Username { get; init; }
        public string Password { get; init; }
        public int? Kills { get; init; }
        public int? Deaths { get; init; }
        public int? TeamKills { get; init; }
        public TimeSpan? Playtime { get; init; }
        public string? FavoredFactions { get; init; }

        public UserDTO(int userId, string username, string password, int? kills, int? deaths, int? teamkills, TimeSpan? playtime, string? factions)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Kills = kills;
            Deaths = deaths;
            TeamKills = teamkills;
            Playtime = playtime;
            FavoredFactions = factions;
        }

        public UserDTO(string username, string password)
        {
            Username = username;
            Password = password;
        }

    }
}