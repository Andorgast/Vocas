namespace logic_layer
{
    public class MessageModel
    {
        public int MessageId { get; private set; }
        public UserModel User { get; private set; }
        public int GroupId { get; private set; }
        public string? BodyText { get; private set; }
        public DateTime Time { get; private set; }

        public MessageModel(int messageId, string bodyText, UserModel user, int groupId)
        {
            MessageId = messageId;
            BodyText = bodyText;
            User = user;
            GroupId = groupId;
        }

        public MessageModel(string bodyText, UserModel user, int groupId)
        {
            BodyText = bodyText;
            User = user;
            GroupId = groupId;
        }

        public bool EditMessage(int editingUser, string bodyText)
        {
            if (editingUser != User.UserId)
            {
                return false;
            }
            else
            {
                BodyText = bodyText;
                return true;
            }
        }
    }
}
