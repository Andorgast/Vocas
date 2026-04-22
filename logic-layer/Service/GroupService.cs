using data_layer;
namespace logic_layer
{
    public class GroupService
    {
        private GroupRepo GroupRepo = new();
        private UserService UserService = new();

        public enum UserRemoving
        {
            user_not_in_group,
            already_voted,
            deleted_succes,
            voted_succes
        }

        //public bool CheckIfUserInGroup(UserModel userToCheck)
        //{
        //    foreach (UserModel user in GroupModel.Users)
        //    {
        //        if (user.UserId == userToCheck.UserId)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public GroupModel? GetGroupById(int id, int requestingUser)
        {
            if (!GroupRepo.CheckIfUserIsInGroup(id, requestingUser))
            {
                return null;
            }
            GroupDTO groupDTO = GroupRepo.GetGroupById(id);
            return new(groupDTO.GroupId, groupDTO.Users);
        }

        public List<GroupModel> GetAllGroupsByUser(int userId)
        {
            List<GroupDTO> groupDTOList = GroupRepo.GetAllGroupsByUser(userId);
            List<GroupModel> groupModelList = [];
            foreach (GroupDTO groupDTO in groupDTOList)
            {
                groupModelList.Add(new(groupDTO.GroupId, groupDTO.Users));
            }
            return groupModelList;
        }

        public UserModel? AddUserToGroup(string username, GroupModel group)
        {
            UserModel? userModel = UserService.GetUserByName(username);
            if (userModel == null || !group.AddUserToGroup(userModel.UserId))
            {
                userModel = null;
                return userModel;
            }
            GroupRepo.AddUserToGroup(new GroupDTO(group.GroupId, group.UserIds), userModel.UserId);
            return userModel;
        }

        public UserRemoving VoteForRemoval(int votingUser, int votedUser, GroupModel group)
        {
            bool votedIsInGroup = false;
            bool votingIsInGroup = false;
            List<int> votingUserIds = GroupRepo.GetRemovalVotes(votingUser, votedUser, group.GroupId);

            foreach (var user in group.UserIds)
            {
                if (user == votedUser)
                {
                    votedIsInGroup = true;
                }
                if (user == votingUser)
                {
                    votingIsInGroup = true;
                }
            }
            if (!votedIsInGroup || !votingIsInGroup)
            {
                return UserRemoving.user_not_in_group;
            }

            foreach (int userId in votingUserIds)
            {
                if (userId == votingUser)
                {
                    return UserRemoving.already_voted;
                }
            }

            votingUserIds.Add(votingUser);
            if ((float)votingUserIds.Count() > ((float)group.UserIds.Count() / 2))
            {
                GroupRepo.RemoveUserFromGroup(votedUser, group.GroupId);
                group.RemoveUser(votedUser);
                return UserRemoving.deleted_succes;
            }

            GroupRepo.AddVoteForRenoval(votingUser, votedUser, group.GroupId);

            return UserRemoving.voted_succes;
        }

        public void RemoveUserFromGroup(int userToRemove, int groupId)
        {
            //THIS FUNCTION IS ONLY TO BE USED TO FORCIBLY REMOVE A USER, VOTE FOR REMOVAL ALREADY REMOVES A USER IF VOTES ARE ENOUGH
            GroupRepo.RemoveUserFromGroup(userToRemove, groupId);
        }

        public GroupModel CreateNewGroup(List<int> users)
        {
            GroupDTO groupDTO = GroupRepo.AddGroupToDb(new GroupDTO(users));
            return new(groupDTO.GroupId, users);
        }
    }
}
