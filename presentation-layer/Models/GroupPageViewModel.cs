namespace presentation_layer.Models
{
    public class GroupPageViewModel
    {
        public int CurrentUserId { get; private set; }
        public GroupViewModel GroupInfo { get; private set; }
        public int GroupCount { get; private set; }
        public List<MessageViewModel> Messages { get; private set; }
        public int? Editing { get; private set; }

        public GroupPageViewModel(int currentUserId, GroupViewModel groupInfo, int groupCount, List<MessageViewModel> messages, int? editing)
        {
            CurrentUserId = currentUserId;
            GroupInfo = groupInfo;
            GroupCount = groupCount;
            Messages = messages;
            Editing = editing;
        }
    }
}
