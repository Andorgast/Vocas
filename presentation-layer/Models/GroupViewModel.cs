namespace presentation_layer.Models
{
    public class GroupViewModel
    {
        public int GroupId { get; private set; }
        public int MaxGroupSize { get; private set; } = 4;
        public List<UserViewModel> Users { get; private set; } = [];

        public GroupViewModel(int groupId, List<UserViewModel> users)
        {
            GroupId = groupId;
            Users = users;
        }
    }
}
