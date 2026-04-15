namespace data_layer
{
    public class MessageDTO
    {
        public int MessageId { get; private set; }
        public int UserId { get; private set; }
        public int GroupId { get; private set; }
        public string BodyText { get; private set; }
        public DateTime Time { get; private set; }

        public MessageDTO(int messageId, string bodyText, int userId, int groupId)
        {
            MessageId = messageId;
            BodyText = bodyText;
            UserId = userId;
            GroupId = groupId;
        }

        public MessageDTO(string bodyText, int userId,  int groupId)
        {
            //for new messages where the id isnt know yet
            BodyText = bodyText;
            UserId = userId;
            GroupId = groupId;
        }

        public void SetMessageId(int messageId)
        {
            //for new messages when they have been sent and the id is known
            MessageId = messageId;
        }

        public void EditText(string newText)
        {
            BodyText = newText;
        }
    }
}
