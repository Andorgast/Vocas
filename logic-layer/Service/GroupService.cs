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
            user_not_in_group,
            already_voted,
            deleted_succes,
            voted_succes
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
                foreach (int groupUserID in GroupRepo.GroupDTO.Users)
                {
                    UserService.GetUserById(groupUserID);
                }
                GroupModelList.Add(new GroupModel(GroupRepo.GroupDTO.GroupId, UserService.UserModeList));
            }
        }

        public void AddUserToGroup(string username)
        {
            UserService.GetUserByName(username);
            GroupModel.AddUserToGroup(UserService.UserModel);
        }

        public UserRemoving VoteForRemoval(int votingUser, int votedUser)
        {
            bool votedIsInGroup = false;
            bool votingIsInGroup = false;
            List<int> votingUsers = GroupRepo.GetRemovalVotes(votingUser, votedUser);

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

            votingUsers.Add(votingUser);
            if ((float)votingUsers.Count() > ((float)GroupModel.Users.Count() / 2))
            {
                GroupRepo.RemoveUserFromGroup(votedUser);
                UserService.GetUserById(votedUser);
                GroupModel.RemoveUser(UserService.UserModel);
                return UserRemoving.deleted_succes;
            }

            GroupRepo.AddVoteForRenoval(votingUser, votedUser);

            return UserRemoving.voted_succes;
        }

        public bool CreateNewGroup(List<int> users)
        {
            GroupRepo.AddGroupToDb(new GroupDTO(users));
            //create the group in the db and add all users to that group
            return true;
        }
    }
}
