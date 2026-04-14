using logic_layer;
namespace logic_layer
{
    public class Message
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";
        public int Id { get; private set; }
        public User User { get; private set; }
        public int GroupId { get; private set; }
        public string BodyText { get; private set; }
        public DateTime Time { get; private set; }

        public Message(int id)
        {
            //get a message from the db based on its id
        }

        public Message(string bodyText, int userId, int groupId)
        {
            BodyText = bodyText;
            User = new User(userId);
            GroupId = groupId;
            SendMessage();
        }

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
