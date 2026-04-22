namespace data_layer
{
    public record MessageDTO
    {
        public int MessageId { get; set; }
        public int UserId { get; init; }
        public int GroupId { get; init; }
        public string BodyText { get; init; }
        public DateTime Time { get; init; }

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
