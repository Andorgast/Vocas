namespace logic_layer
{
    public class MessageModel
    {
        public int MessageId { get; private set; }
        public int UserId { get; private set; }
        public int GroupId { get; private set; }
        public string BodyText { get; private set; }
        public DateTime Time { get; private set; }

        public MessageModel(int messageId, string bodyText, int userId, int groupId)
        {
            MessageId = messageId;
            BodyText = bodyText;
            UserId = userId;
            GroupId = groupId;
        }

        public MessageModel(string bodyText, int userId, int groupId)
        {
            BodyText = bodyText;
            UserId = userId;
            GroupId = groupId;
        }

        public bool EditMessage(int editingUser, string bodyText)
        {
            if (editingUser != UserId)
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
