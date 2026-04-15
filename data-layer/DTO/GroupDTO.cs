namespace data_layer
{
    public class GroupDTO
    {
        public int GroupId { get; private set; }
        public int MaxGroupSize { get; private set; } = 4;
        public List<int> Users { get; private set; } = [];

        public GroupDTO(int groupId, List<int> users)
        {
            GroupId = groupId;
            Users = users;
        }

        public GroupDTO(List<int> users)
        {
            Users = users;
        }

        public void RemoveUser(int userToRemove)
        {
            Users.Remove(userToRemove);
        }

        public void AddGroupId(int groupId)
        {
            //for when a group is created and the id needs to be added
            GroupId = groupId;
        }
    }
}
