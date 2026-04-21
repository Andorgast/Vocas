namespace data_layer
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string BodyText { get; set; }
        public DateTime Time { get; set; }

        public MessageDTO(int messageId, string bodyText, int userId, int groupId)
        {
            MessageId = messageId;
            BodyText = bodyText;
            UserId = userId;
            GroupId = groupId;
        }

        public MessageDTO(string bodyText, int userId, int groupId)
        {
            //for new messages where the id isnt know yet
            BodyText = bodyText;
            UserId = userId;
            GroupId = groupId;
        }
    }
}
