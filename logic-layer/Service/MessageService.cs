using data_layer;
using Google.Protobuf;
namespace logic_layer
{
    public class MessageService
    {
        private MessageRepo MessageRepo = new();
        private UserService UserService = new();

        public List<MessageModel> GetAllMessagesByGroup(int groupId)
        {
            List<MessageModel> messageModelList = [];
            List<MessageDTO> messageDTOList = MessageRepo.GetAllMessagesByGroup(groupId);
            foreach (MessageDTO messageDTO in messageDTOList)
            {
                messageModelList.Add(new(messageDTO.MessageId, messageDTO.BodyText, messageDTO.UserId, messageDTO.GroupId));
            }
            return messageModelList;
        }

        public MessageModel GetMessageById(int messageId)
        {
            MessageDTO messageDTO = MessageRepo.GetMessageById(messageId);
            return new(messageDTO.MessageId, messageDTO.BodyText, messageDTO.UserId, messageDTO.GroupId);
        }

        public void SendMessage(MessageModel message)
        {
            MessageRepo.SendMessage(new(message.BodyText, message.UserId, message.GroupId));
        }

        public void DeleteMessage(int deletingUser, int messageId)
        {
            MessageModel oldMessage = GetMessageById(messageId);
            if (oldMessage.UserId == deletingUser)
            {
                MessageRepo.DeleteMessage(messageId);
            }
            
        }

        public void EditMessage(MessageModel message)
        {
            MessageModel oldMessage = GetMessageById(message.MessageId);
            if (oldMessage.UserId == message.UserId)
            {
                MessageRepo.EditMessage(new(message.MessageId, message.BodyText, message.UserId, message.GroupId));
            }
        }
    }
}
