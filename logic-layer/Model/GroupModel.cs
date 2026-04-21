namespace logic_layer
{
    public class GroupModel
    {
        public int GroupId { get; private set; }
        public int MaxGroupSize { get; private set; } = 4;
        public List<int> UserIds { get; private set; } = [];

        public GroupModel(int groupId, List<int> users)
        {
            GroupId = groupId;
            UserIds = users;
        }

        public bool AddUserToGroup(int userToAdd)
        {
            if (UserIds.Count() > (MaxGroupSize - 1))
            {
                return false;
            }
            foreach (var user in UserIds)
            {
                if (user == userToAdd)
                {
                    return false;
                }
            }
            UserIds.Add(userToAdd);
            return true;
        }

        public void RemoveUser(int userToRemove)
        {
            UserIds.Remove(userToRemove);
        }
    }
}
