using data_layer;
namespace logic_layer
{
    public class UserService
    {
        private UserRepo UserRepo = new();
        private AvailabilityRepo AvailabilityRepo = new();
        public List<UserModel> UserModeList { get; private set; } = [];
        public UserModel UserModel { get; private set; }
        public List<AvailabilityModel> AvailabilityModelList { get; private set; } = [];

        public void GetUserById(int userId)
        {
            UserRepo.GetUserById(userId);
            UserModeList.Add(new UserModel(UserRepo.UserDTO.UserId, UserRepo.UserDTO.Username, UserRepo.UserDTO.Kills, UserRepo.UserDTO.Deaths, UserRepo.UserDTO.TeamKills, UserRepo.UserDTO.Playtime, UserRepo.UserDTO.FavoredFactions));
            UserModel = new UserModel(UserRepo.UserDTO.UserId, UserRepo.UserDTO.Username, UserRepo.UserDTO.Kills, UserRepo.UserDTO.Deaths, UserRepo.UserDTO.TeamKills, UserRepo.UserDTO.Playtime, UserRepo.UserDTO.FavoredFactions);
        }

        public void GetUserByName(string username)
        {
            UserRepo.GetUserByName(username);
            UserModel = new UserModel(UserRepo.UserDTO.UserId, UserRepo.UserDTO.Username, UserRepo.UserDTO.Kills, UserRepo.UserDTO.Deaths, UserRepo.UserDTO.TeamKills, UserRepo.UserDTO.Playtime, UserRepo.UserDTO.FavoredFactions);
        }

        public void GetAllUsers()
        {
            UserRepo.GetAllUsers();
            foreach(UserDTO userDTO in UserRepo.UserDTOList)
            {
                UserModeList.Add(new UserModel(userDTO.UserId, userDTO.Username, userDTO.Kills, userDTO.Deaths, userDTO.TeamKills, userDTO.Playtime, userDTO.FavoredFactions));
            }
        }

        public string? AddFavoredFaction(List<string> factionsToAdd)
        {
            string factionsBefore = UserModel.FavoredFactions;
            string? duplicates = UserModel.AddFavoredFaction(factionsToAdd);
            if(factionsBefore != UserModel.FavoredFactions)
            {
                UserRepo.UpdateUserData();
            }
            return duplicates;
        }

        public void RemoveDayAvailable(int id)
        {
            if (UserModel.RemoveDayAvailable(id))
            {
                foreach (AvailabilityModel availabilityModel in AvailabilityModelList)
                {
                    if (availabilityModel.Id == id)
                    {
                        AvailabilityRepo.RemoveAvailability(availabilityModel.Id);
                    }
                }
            }
        }

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
