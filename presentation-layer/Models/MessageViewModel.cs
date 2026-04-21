namespace presentation_layer.Models
{
    public class MessageViewModel
    {
        public int MessageId { get; private set; }
        public UserViewModel User { get; private set; }
        public int GroupId { get; private set; }
        public string BodyText { get; private set; }
        public DateTime Time { get; private set; }

        public MessageViewModel(int messageId, string bodyText, UserViewModel user, int groupId)
        {
            MessageId = messageId;
            BodyText = bodyText;
            User = user;
            GroupId = groupId;
        }
    }
}
