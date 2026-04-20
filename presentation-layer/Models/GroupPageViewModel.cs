using Google.Protobuf.Reflection;

namespace presentation_layer.Models
{
    public class GroupPageViewModel
    {
        public int CurrentUserId { get; private set; }
        public GroupViewModel? GroupInfo { get; private set; }
        public int GroupCount { get; private set; }
        public List<GroupViewModel>? UserGroups { get; private set; }
        public List<MessageViewModel>? Messages { get; private set; }
        public int? Editing { get; private set; }

        public GroupPageViewModel(int currentUserId, GroupViewModel? groupInfo, int groupCount, List<MessageViewModel>? messages, int? editing)
        {
            CurrentUserId = currentUserId;
            GroupInfo = groupInfo;
            GroupCount = groupCount;
            Messages = messages;
            Editing = editing;
            UserGroups = null;
        }

        public GroupPageViewModel(int currentUserId, List<GroupViewModel> userGroups)
        {
            CurrentUserId = currentUserId;
            UserGroups = userGroups;
            GroupCount = 0;
            GroupInfo = null;
            Editing = null;
            Messages = null;
        }
    }
}
