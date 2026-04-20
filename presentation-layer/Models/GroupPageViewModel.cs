namespace presentation_layer.Models
{
    public class GroupPageViewModel
    {
        public int CurrentUserId { get; private set; }
        public GroupViewModel? GroupInfo { get; private set; }
        public int GroupCount { get; private set; }
        public List<GroupViewModel>? UserGroups = [];
        public List<MessageViewModel>? Messages = [];
        public int? Editing { get; private set; }

        public GroupPageViewModel(int currentUserId, GroupViewModel? groupInfo, int groupCount, List<GroupViewModel>? userGroups, List<MessageViewModel>? messages, int? editing)
        {
            CurrentUserId = currentUserId;
            GroupInfo = groupInfo;
            GroupCount = groupCount;
            UserGroups = userGroups;
            Messages = messages;
            Editing = editing;
        }
    }
}
