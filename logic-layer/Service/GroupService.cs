using data_layer;

namespace logic_layer
{
    public class GroupService
    {
        private GroupRepo GroupRepo = new();
        private UserService UserService = new();
        public List<GroupModel> GroupModelList { get; private set; } = [];
        public GroupModel GroupModel { get; private set; }
        public enum UserRemoving
        {
            null_value,
            user_not_in_group,
            already_voted,
            deleted_succes,
            voted_succes
        }

        public bool CheckIfUserInGroup(UserModel userToCheck)
        {
            foreach (UserModel user in GroupModel.Users)
            {
                if (user.UserId == userToCheck.UserId)
                {
                    return true;
                }
            }
            return false;
        }

        public void GetGroupById(int id)
        {
            GroupRepo.GetGroupById(id);
            foreach (int userId in GroupRepo.GroupDTO.Users)
            {
                UserService.GetUserById(userId);
            }
            GroupModel = new GroupModel(GroupRepo.GroupDTO.GroupId, UserService.UserModeList);
        }

        public void GetAllGroupsByUser(int userId)
        {
            GroupRepo.GetAllGroupsByUser(userId);
            foreach (GroupDTO groupDTO in GroupRepo.GroupDTOList)
            {
                UserService = new();
                foreach (int groupUserID in groupDTO.Users)
                {
                    UserService.GetUserById(groupUserID);
                }
                GroupModelList.Add(new GroupModel(groupDTO.GroupId, UserService.UserModeList));
            }
            GroupModel = new GroupModel(GroupRepo.GroupDTO.GroupId, UserService.UserModeList);
        }

        public bool AddUserToGroup(string? username)
        {
            if (username != null)
            {
                if (!UserService.GetUserByName(username))
                {
                    return false;
                }
                GroupModel.AddUserToGroup(UserService.UserModel);

                List<int> tempIdList = [];
                foreach (UserModel user in GroupModel.Users)
                {
                    tempIdList.Add(user.UserId);
                }
                GroupRepo.AddUserToGroup(new GroupDTO(GroupModel.GroupId, tempIdList), UserService.UserModel.UserId);
                return true;
            }
            return false;
        }

        public UserRemoving VoteForRemoval(int? votingUser, int? votedUser)
        {
            bool votedIsInGroup = false;
            bool votingIsInGroup = false;
            if (votingUser == null || votedUser == null)
            {
                return UserRemoving.null_value;
            }
            List<int> votingUsers = GroupRepo.GetRemovalVotes((int)votingUser, (int)votedUser);

            foreach (var user in GroupModel.Users)
            {
                if (user.UserId == votedUser)
                {
                    votedIsInGroup = true;
                }
                if (user.UserId == votingUser)
                {
                    votingIsInGroup = true;
                }
            }
            if (!votedIsInGroup || !votingIsInGroup)
            {
                return UserRemoving.user_not_in_group;
            }

            foreach (int userId in votingUsers)
            {
                if (userId == votingUser)
                {
                    return UserRemoving.already_voted;
                }
            }

            votingUsers.Add((int)votingUser);
            if ((float)votingUsers.Count() > ((float)GroupModel.Users.Count() / 2))
            {
                GroupRepo.RemoveUserFromGroup((int)votedUser);
                UserService.GetUserById((int)votedUser);
                GroupModel.RemoveUser(UserService.UserModel);
                return UserRemoving.deleted_succes;
            }

            GroupRepo.AddVoteForRenoval((int)votingUser, (int)votedUser);

            return UserRemoving.voted_succes;
        }

        public bool CreateNewGroup(List<int> users)
        {
            GroupRepo.AddGroupToDb(new GroupDTO(users));
            UserService userServiceTemp = new();
            List<UserModel> userModelListTemp = [];
            foreach (int userId in users)
            {
                userServiceTemp.GetUserById(userId);
                userModelListTemp.Add(userServiceTemp.UserModel);
            }
            GroupModel = new GroupModel(GroupRepo.GroupDTO.GroupId, userModelListTemp);
            return true;
        }
    }
}
