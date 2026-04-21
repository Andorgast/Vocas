namespace logic_layer
{
    public class GroupModel
    {
        public int GroupId { get; private set; }
        public int MaxGroupSize { get; private set; } = 4;
        public List<UserModel> Users { get; private set; } = [];

        public GroupModel(int groupId, List<UserModel> users)
        {
            GroupId = groupId;
            Users = users;
        }

        public bool AddUserToGroup(UserModel userToAdd)
        {
            if (Users.Count() > (MaxGroupSize - 1))
            {
                return false;
            }
            foreach (var user in Users)
            {
                if (user.UserId == userToAdd.UserId)
                {
                    return false;
                }
            }
            Users.Add(userToAdd);
            return true;
        }

        public void RemoveUser(UserModel userToRemove)
        {
            Users.Remove(userToRemove);
        }
    }
}
