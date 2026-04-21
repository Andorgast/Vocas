using data_layer;
namespace logic_layer
{
    public class UserService
    {
        private UserRepo UserRepo = new();
        private AvailabilityRepo AvailabilityRepo = new();

        public UserModel? GetUserById(int userId)
        {
            UserDTO? userDTO  = UserRepo.GetUserById(userId);
            if (userDTO == null)
            {
                return null;
            }
            return new(userDTO.UserId, userDTO.Username, userDTO.Kills, userDTO.Deaths, userDTO.TeamKills, userDTO.Playtime, userDTO.FavoredFactions);
        }

        public UserModel? GetUserByName(string username)
        {
            UserDTO? userDTO = UserRepo.GetUserByName(username);
            if (userDTO == null)
            {
                return null;
            }
            return new(userDTO.UserId, userDTO.Username, userDTO.Kills, userDTO.Deaths, userDTO.TeamKills, userDTO.Playtime, userDTO.FavoredFactions);
        }

        public List<UserModel> GetAllUsers()
        {
            List<UserDTO> userDTOList = UserRepo.GetAllUsers();
            List<UserModel> userModelList = [];
            foreach (UserDTO userDTO in userDTOList)
            {
                userModelList.Add(new UserModel(userDTO.UserId, userDTO.Username, userDTO.Kills, userDTO.Deaths, userDTO.TeamKills, userDTO.Playtime, userDTO.FavoredFactions));
            }
            return userModelList;
        }

        //public string? AddFavoredFaction(List<string> factionsToAdd, UserModel userModel)
        //{
        //    string factionsBefore = userModel.FavoredFactions;
        //    string? duplicates = userModel.AddFavoredFaction(factionsToAdd);
        //    if (factionsBefore != userModel.FavoredFactions)
        //    {
        //        UserRepo.UpdateUserData();
        //    }
        //    return duplicates;
        ////}

        //public bool DayAvailableChange(AvailabilityModel newAvailability)
        //{
        //    foreach (var moment in Availability)
        //    {
        //        if (moment.Day == newAvailability.Day)
        //        {
        //            moment.StartTime = newAvailability.StartTime;
        //            moment.EndTime = newAvailability.EndTime;
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public void AddNewDayAvailable(string dayToAdd, TimeSpan startTime, TimeSpan endTime, int id)
        //{
        //    Availability.Add(new AvailabilityModel(id, dayToAdd, startTime, endTime));
        //}

        //public void RemoveDayAvailable(int availableId)
        //{
        //    AvailabilityRepo.RemoveAvailability(availableId);
        //}

        //public void UpdateAvailability(int id)
        //{
        //    AvailabilityRepo.GetAvailabilityById(id);
        //}

        //public void AddDayAvailable()
        //{
        //    AvailabilityRepo.AddAvailability();
        //}
    }
}
