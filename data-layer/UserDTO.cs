namespace data_layer
{
    public class UserDTO
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Kills { get; private set; }
        public int Deaths { get; private set; }
        public int TeamKills { get; private set; }
        public TimeSpan Playtime { get; private set; }
        public string FavoredFactions { get; private set; }
        public int UserId { get; private set; }

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