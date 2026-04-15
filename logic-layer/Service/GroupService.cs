namespace logic_layer
{
    public class GroupService
    {
        public UserAdding AddUserToGroup(string username)
        {
            if (Users.Count() > (MaxGroupSize - 1))
            {
                return UserAdding.group_is_full;
            }

            //get user based on username from db
            var userId = 2;

            foreach (var user in Users)
            {
                if (user.UserId == userId)
                {
                    return UserAdding.user_already_in_group;
                }
            }
            
            //add user to group in db

            Users.Add(new User(userId));
            return UserAdding.success;
        }

        public UserRemoving VoteForRemoval(int votingUser, int votedUser)
        {
            bool votedIsInGroup = false;
            bool votingIsInGroup = false;
            List<int> votingUsers = [];
            foreach (var user in Users)
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
            
            //get all id's of users who voted on votedUser

            foreach(int userId in votingUsers)
            {
                if(userId == votingUser)
                {
                    return UserRemoving.already_voted;
                }
            }
            votingUsers.Add(votingUser);
            if ((float)votingUsers.Count() > ((float)Users.Count()/2))
            {
                //delete user from group, remove their votes in the group and remove the votes for them in the group

                UserService? tempUserContainer = null;
                foreach(var user in Users)
                {
                    if (user.UserId == votedUser) 
                    {
                        tempUserContainer = user;
                    }
                }
                if (tempUserContainer != null)
                {
                    Users.Remove(tempUserContainer);
                }
                else
                {
                    return UserRemoving.user_not_in_group;
                }
                return UserRemoving.deleted_succes;
            }
            
            //add the vote to the db

            return UserRemoving.voted_succes;
        }

        public bool AddGroupToDb()
        {
            //create the group in the db and add all users to that group
            return true;
        }
    }
}
