using data_layer;
namespace logic_layer
{
    public class UserService
    {
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
            AvailabilityService? dayUpdater = null;
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
