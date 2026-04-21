namespace data_layer
{
    public class GroupDTO
    {
        public int GroupId { get; set; }
        public int MaxGroupSize { get; set; } = 4;
        public List<int> Users { get; set; } = [];

        public GroupDTO(int groupId, List<int> users)
        {
            GroupId = groupId;
            Users = users;
        }

        public GroupDTO(List<int> users)
        {
            Users = users;
        }
    }
}
