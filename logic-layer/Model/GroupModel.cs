using data_layer;

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

        public bool AddUserToGroup(int userId)
        {
            if (Users.Count() > (MaxGroupSize - 1))
            {
                return false;
            }
            foreach (var user in Users)
            {
                if (user.UserId == userId)
                {
                    return false;
                }
            }
            Users.Add(new UserModel(userId));
            return true;
        }
    }
}
