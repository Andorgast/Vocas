namespace data_layer
{
    public record UserDTO
    {
        public string Username { get; init; }
        public string Password { get; init; }
        public int Kills { get; init; }
        public int Deaths { get; init; }
        public int TeamKills { get; init; }
        public TimeSpan Playtime { get; init; }
        public string FavoredFactions { get; init; }
        public int UserId { get; init; }

        public UserDTO(int userId, string username, string password, int kills, int deaths, int teamkills, string playtime, string factions)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Kills = kills;
            Deaths = deaths;
            TeamKills = teamkills;
            Playtime = TimeSpan.Parse(playtime);
            FavoredFactions = factions;
        }
    }
}