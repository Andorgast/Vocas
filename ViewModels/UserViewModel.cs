using MySql.Data.MySqlClient;
using System.Reflection.PortableExecutable;

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
        public string FavoredFactions { get; private set; }
        public int UserId { get; private set; }

        public UserViewModel(int userId)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM users WHERE id=@id", conn
            );
            cmd.Parameters.AddWithValue("@id", userId);
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return;
            }
            UserId = reader.GetInt32(0);
            Username = reader.GetString(1);
            Kills = reader.GetInt32(3);
            Deaths = reader.GetInt32(4);
            TeamKills = reader.GetInt32(5);
            Playtime = TimeSpan.Parse(reader.GetString(6));
            FavoredFactions = reader.GetString(7);
            conn.Close();
            conn.Open();
            cmd = new MySqlCommand(
                @"SELECT * FROM availability WHERE user_id=@id", conn    
            );
            cmd.Parameters.AddWithValue("@id", UserId);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Available.Add(new AvailabilityViewModel(reader.GetString(2), reader.GetTimeSpan(3), reader.GetTimeSpan(4)));
            }
            conn.Close();
            return;
        }

        public UserViewModel(int userId, string username, int kills, int deaths, int teamkills, string playtime, string factions)
        {
            UserId = userId;
            Username = username;
            Kills = kills;
            Deaths = deaths;
            TeamKills = teamkills;
            Playtime = TimeSpan.Parse(playtime);
            FavoredFactions = factions;
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM availability WHERE user_id=@id", conn
            );
            cmd.Parameters.AddWithValue("@id", UserId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Available.Add(new AvailabilityViewModel(reader.GetString(2), reader.GetTimeSpan(3), reader.GetTimeSpan(4)));
            }
            conn.Close();
        }

        public bool AddFavoredFaction(List<string> factionsToAdd)
        {
            bool noDuplicates = true;
            bool changedFaction = false;
            foreach (string faction in factionsToAdd)
            {
                if (FavoredFactions.Contains(faction) || FavoredFactions == "all")
                {
                    noDuplicates = false;
                }
                else if (FavoredFactions.Contains("&"))
                {
                    FavoredFactions = "all";
                    changedFaction = true;
                }
                else
                {
                    //bots are always behind the & sign, squids always in front, bugs adapt based on the other faction
                    if(FavoredFactions == "bots" || faction == "squids")
                    {
                        FavoredFactions = faction + " & " + FavoredFactions;
                    }
                    else if (FavoredFactions == "squids" || faction == "bots")
                    {
                        FavoredFactions = FavoredFactions + " & " + faction;
                    }
                    changedFaction = true;
                }
            }
            if (changedFaction)
            {
                var conn = new MySqlConnection(connectionString);
                conn.Open();
                var cmd = new MySqlCommand(
                    @"UPDATE users WHERE id = @id SET faction = @factions", conn
                );
                cmd.Parameters.AddWithValue("@id", UserId);
                cmd.Parameters.AddWithValue("@factions", FavoredFactions);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return noDuplicates;
        }

        public bool DayAvailableChange(string dayToChange, TimeSpan newStartTime, TimeSpan newEndTime, bool newDay)
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
                    new AvailabilityViewModel(dayToChange, newStartTime, newEndTime)
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
