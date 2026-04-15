using data_layer;
namespace logic_layer
{
    public class MessageService
    {
        public bool SendMessage()
        {
            //send the message to the db
            return true;
        }

        public bool DeleteMessage(int deletingUser)
        {
            if(deletingUser != User.UserId)
            {
                return false;
            }
            else
            {
                //delete a message from the db
                return true;
            }
        }

        public bool EditMessage(int editingUser, string bodyText)
        {
            if (editingUser != User.UserId)
            {
                return false;
            }
            else
            {
                //edit a message in the db
                return true;
            }
        }
    }
}
