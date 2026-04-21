namespace data_layer
{
    public record GroupDTO
    {
        public int GroupId { get; set; }
        public int MaxGroupSize { get; init; } = 4;
        public List<int> Users { get; init; } = [];

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
