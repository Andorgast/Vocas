using data_layer;
namespace logic_layer
{
    public class UserService
    {
        private UserRepo UserRepo = new();
        private AvailabilityRepo AvailabilityRepo = new();

        public static bool CheckPasswordByCriteria(string password)
        {
            if (password.Length < 8)
            {
                //password too short
                return false;
            }
            else if (password.ToLower() == password)
            {
                //no capital letter
                return false;
            }
            else
            {
                return true;
            }
        }

        public UserModel? GetUserById(int userId)
        {
            UserDTO? userDTO  = UserRepo.GetUserById(userId);
            if (userDTO == null)
            {
                return null;
            }
            List<AvailabilityDTO> availabilityDTOList = AvailabilityRepo.GetAllAvailabilityByUser(userId);
            List<AvailabilityModel> availabilityModelList = [];
            foreach (AvailabilityDTO availability in availabilityDTOList)
            {
                availabilityModelList.Add(new(availability.Id, availability.Day, availability.StartTime, availability.EndTime));
            }
            return new((int)userDTO.UserId, userDTO.Username, userDTO.Password, availabilityModelList, userDTO.Kills, userDTO.Deaths, userDTO.TeamKills, userDTO.Playtime, userDTO.FavoredFactions);
        }

        public UserModel? CreateNewUser(UserModel userInfo)
        {
            if (UserRepo.GetUserByName(userInfo.Username) != null)
            {
                return null;
            }
            //if id is null here it has crashed and burned too hard to recover
            userInfo.SetId((int)UserRepo.CreateNewUser(new(userInfo.Username, userInfo.GetPassword())).UserId);
            return userInfo;
        }

        public UserModel? GetUserByName(string username)
        {
            UserDTO? userDTO = UserRepo.GetUserByName(username);
            if (userDTO == null)
            {
                return null;
            }
            //userId is not null here, database doesnt allow it
            List<AvailabilityDTO> availabilityDTOList = AvailabilityRepo.GetAllAvailabilityByUser((int)userDTO.UserId);
            List<AvailabilityModel> availabilityModelList = [];
            foreach (AvailabilityDTO availability in availabilityDTOList)
            {
                availabilityModelList.Add(new(availability.Id, availability.Day, availability.StartTime, availability.EndTime));
            }
            return new((int)userDTO.UserId, userDTO.Username, userDTO.Password, availabilityModelList, userDTO.Kills, userDTO.Deaths, userDTO.TeamKills, userDTO.Playtime, userDTO.FavoredFactions);
        }

        public List<UserModel> GetAllUsers(int userToExclude)
        {
            List<UserDTO> userDTOList = UserRepo.GetAllUsers(userToExclude);
            List<UserModel> userModelList = [];
            foreach (UserDTO userDTO in userDTOList)
            {
                //UserId is again not null here, never, only if the database fails to check if a user needs an id is this even possible and even then it would crash and burn when trying to get that id
                List<AvailabilityDTO> availabilityDTOList = AvailabilityRepo.GetAllAvailabilityByUser((int)userDTO.UserId);
                List<AvailabilityModel> availabilityModelList = [];
                foreach (AvailabilityDTO availability in availabilityDTOList)
                {
                    availabilityModelList.Add(new(availability.Id, availability.Day, availability.StartTime, availability.EndTime));
                }
                userModelList.Add(new((int)userDTO.UserId, userDTO.Username, userDTO.Password, availabilityModelList, userDTO.Kills, userDTO.Deaths, userDTO.TeamKills, userDTO.Playtime, userDTO.FavoredFactions));
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
