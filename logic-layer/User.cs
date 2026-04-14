using logic_layer;
namespace logic_layer
{
    public class User
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";
        public string Username { get; set; } = "";
        //private string Password = "";
        public List<Availability> Available { get; private set; } = new();
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int TeamKills { get; set; }
        public TimeSpan Playtime { get; set; }
        public string FavoredFactions { get; private set; }
        public int UserId { get; private set; }

        public User(int userId)
        {
            //get user and availability from db based on user id
        }

        public User(int userId, string username, int kills, int deaths, int teamkills, string playtime, string factions)
        {
            UserId = userId;
            Username = username;
            Kills = kills;
            Deaths = deaths;
            TeamKills = teamkills;
            Playtime = TimeSpan.Parse(playtime);
            FavoredFactions = factions;
            //get availability from db
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
                //update db favored faction
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
                    new Availability(dayToChange, newStartTime, newEndTime)
                );
                return true;
            }
            return false;
        }

        public bool RemoveDayAvailable(string dayToChange)
        {
            Availability? dayUpdater = null;
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
