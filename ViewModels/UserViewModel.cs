using MySql.Data.MySqlClient;

namespace Vocas.ViewModels
{
    public class UserViewModel
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";
        public string Username { get; set; } = "";
        //private string Password = "";
        public List<AvailabilityViewModel> Available { get; private set; } = new();
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int TeamKills { get; set; }
        public TimeSpan Playtime { get; set; }
        public List<string> FavoredFactions { get; private set; } = new();
        public int? UserId { get; private set; }

        public UserViewModel(int userId)
        {
            GetSpecificUser(userId);
        }

        public UserViewModel(int userId, string username, int kills, int deaths, int teamkills, string playtime, List<string> factions)
        {
            UserId = userId;
            Username = username;
            Kills = kills;
            Deaths = deaths;
            TeamKills = teamkills;
            Playtime = TimeSpan.Parse(playtime);
            AddFavoredFaction(factions);
        }

        public async void GetSpecificUser(int userId)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM users WHERE id=@id", conn
            );
            cmd.Parameters.AddWithValue("@id", userId);
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return;
            }
            UserId = reader.GetInt32(0);
            Username = reader.GetString(1);
            Kills = reader.GetInt32(3);
            Deaths = reader.GetInt32(4);
            TeamKills = reader.GetInt32(5);
            Playtime = TimeSpan.Parse(reader.GetString(6));
            return;
        }

        public bool AddFavoredFaction(List<string> factionsToAdd)
        {
            foreach (string faction in factionsToAdd)
            {
                foreach (string factionAdded in FavoredFactions)
                {
                    if (factionAdded == faction)
                    {
                        return false;
                    }
                }
                FavoredFactions.Add(faction);
            }

            return true;
        }

        public bool DayAvailableChange(string dayToChange, TimeOnly newStartTime, TimeOnly newEndTime, bool newDay)
        {
            foreach (var dayAvailable in Available)
            {
                if (dayAvailable.Day == dayToChange && !newDay)
                {
                    dayAvailable.StartTime = newStartTime;
                    dayAvailable.EndTime = newEndTime;
                    return true;
                }
                else if (dayAvailable.Day == dayToChange && newDay)
                {
                    return false;
                }
            }
            if (newDay)
            {
                Available.Add(
                    new AvailabilityViewModel
                    {
                        Day = dayToChange,
                        StartTime = newStartTime,
                        EndTime = newEndTime
                    }
                );
                return true;
            }
            return false;
        }

        public bool RemoveDayAvailable(string dayToChange)
        {
            AvailabilityViewModel? dayUpdater = null;
            foreach (var dayAvailable in Available)
            {
                if (dayAvailable.Day == dayToChange)
                {
                    dayUpdater = dayAvailable;
                }
            }
            if (dayUpdater != null)
            {
                Available.Remove(dayUpdater);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
