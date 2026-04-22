namespace logic_layer
{
    public class UserModel
    {
        public string Username { get; private set; } = "";
        //private string Password = "";
        public List<AvailabilityModel> Availability { get; private set; } = [];
        public int Kills { get; private set; }
        public int Deaths { get; private set; }
        public int TeamKills { get; private set; }
        public TimeSpan Playtime { get; private set; }
        public string FavoredFactions { get; private set; }
        public int UserId { get; private set; }

        public UserModel(int userId, string username, int kills, int deaths, int teamkills, TimeSpan playtime, string factions)
        {
            UserId = userId;
            Username = username;
            Kills = kills;
            Deaths = deaths;
            TeamKills = teamkills;
            Playtime = playtime;
            FavoredFactions = factions;
            //get availability from db
        }

        //public string? AddFavoredFaction(List<string> factionsToAdd)
        //{
        //    string? duplicates = null;
        //    foreach (string faction in factionsToAdd)
        //    {
        //        if (FavoredFactions.Contains(faction) || FavoredFactions == "all")
        //        {
        //            duplicates += faction;
        //        }
        //        else if (FavoredFactions.Contains("&"))
        //        {
        //            FavoredFactions = "all";
        //        }
        //        else
        //        {
        //            //bots are always behind the & sign, squids always in front, bugs adapt based on the other faction
        //            if (FavoredFactions == "bots" || faction == "squids")
        //            {
        //                FavoredFactions = faction + " & " + FavoredFactions;
        //            }
        //            else if (FavoredFactions == "squids" || faction == "bots")
        //            {
        //                FavoredFactions = FavoredFactions + " & " + faction;
        //            }
        //        }
        //    }
        //    return duplicates;
        //}

        public decimal GetKD()
        {
            decimal KD;
            if (Deaths > 0)
            {
                KD = Math.Round(((decimal)Kills / (decimal)Deaths), 1);
            }
            else
            {
                KD = (decimal)Kills;
            }
            return KD;
        }
    }
}
