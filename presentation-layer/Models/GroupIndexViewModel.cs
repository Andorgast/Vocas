namespace presentation_layer.Models
{
    public class GroupIndexViewModel
    {
        public int CurrentUserId { get; private set; }
        public List<GroupViewModel> UserGroups { get; private set; }

        public GroupIndexViewModel(int currentUserId, List<GroupViewModel> userGroups)
        {
            CurrentUserId = currentUserId;
            UserGroups = userGroups;
        }
    }
}
