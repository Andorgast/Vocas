using data_layer;
namespace logic_layer
{
    public class MessageService
    {
        private MessageRepo MessageRepo = new();
        private UserService UserService = new();
        public List<MessageModel> MessageModelList { get; private set; } = [];

        public void GetAllMessagesByGroup(int groupId)
        {
            MessageRepo.GetAllMessagesByGroup(groupId);
            foreach (MessageDTO messageDTO in MessageRepo.MessageDTOList)
            {
                UserService.GetUserById(messageDTO.UserId);
                MessageModelList.Add(new MessageModel(messageDTO.MessageId, messageDTO.BodyText, UserService.UserModel, messageDTO.GroupId));
            }
        }

        public bool SendMessage(string? bodyText, int groupId, int userId)
        {
            if (bodyText != null)
            {
                MessageRepo.SendMessage(new MessageDTO(bodyText, userId, groupId));
                return true;
            }
            return false;
        }

        public bool DeleteMessage(int deletingUser, int messageId)
        {
            foreach (MessageModel messageModel in MessageModelList)
            {
                if (messageModel.MessageId == messageId)
                {
                    if (deletingUser != messageModel.User.UserId)
                    {
                        return false;
                    }
                    else
                    {
                        MessageRepo.DeleteMessage(messageId);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool EditMessage(int editingUser, string? bodyText, int? messageId)
        {
            if (bodyText != null && messageId != null)
            {
                foreach (MessageModel messageModel in MessageModelList)
                {
                    if (messageModel.MessageId == messageId)
                    {
                        if (editingUser != messageModel.User.UserId)
                        {
                            return false;
                        }
                        else
                        {
                            MessageRepo.EditMessage(new MessageDTO(messageModel.MessageId, messageModel.BodyText, messageModel.User.UserId, messageModel.GroupId), bodyText);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
