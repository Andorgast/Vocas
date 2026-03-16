namespace Vocas.ViewModels
{
    public class UserViewModel
    {
        public string Username { get; set; } = "";
        //private string Password = "";
        public List<AvailabilityViewModel> Available { get; private set; } = new();
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int TeamKills { get; set; }
        public TimeSpan Playtime { get; set; }
        public List<string> FavoredFactions { get; private set; } = new();
        public int? FileRow { get; private set; }

        public UserViewModel(int userId)
        {
            // an example userLine looks like this: "Harold,monday/18:00/20:00.thursday/13:00/15:00,23,200,4543,13:21:22,bots.squids"
            string UserLine = "";
            if(userId < File.ReadLines("users.txt").Count())
            {
                UserLine = File.ReadLines("users.txt").ElementAt(userId);

                Username = UserLine.Split(",")[0];
                foreach (var dayAvailable in UserLine.Split(",")[1].Split("."))
                {
                    DayAvailableChange(dayAvailable.Split("/")[0], TimeOnly.Parse(dayAvailable.Split("/")[1]), TimeOnly.Parse(dayAvailable.Split("/")[2]), true);
                }
                Kills = int.Parse(UserLine.Split(",")[2]);
                Deaths = int.Parse(UserLine.Split(",")[3]);
                TeamKills = int.Parse(UserLine.Split(",")[4]);
                Playtime = TimeSpan.Parse(UserLine.Split(",")[5]);
                foreach (var faction in UserLine.Split(",")[6].Split("."))
                {
                    FavoredFactions.Add(faction);
                }

                FileRow = userId;
            }
            else
            {
                FileRow = null;
            }
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
