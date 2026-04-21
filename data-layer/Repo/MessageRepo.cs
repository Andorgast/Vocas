using MySql.Data.MySqlClient;
namespace data_layer
{
    public class MessageRepo
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";
        public List<MessageDTO> MessageDTOList { get; private set; } = [];

        public List<MessageDTO> GetAllMessagesByGroup(int groupId)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM messages WHERE group_id=@groupId", conn
            );
            cmd.Parameters.AddWithValue("@groupId", groupId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                MessageDTOList.Add(new MessageDTO(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3)));
            }
            conn.Close();
            return MessageDTOList;
        }

        public MessageDTO SendMessage(MessageDTO messageDTO)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"INSERT INTO messages (text, user_id, group_id) VALUE (@bodyText, @userId, @groupId); SELECT LAST_INSERT_ID();", conn
            );
            cmd.Parameters.AddWithValue("@bodyText", messageDTO.BodyText);
            cmd.Parameters.AddWithValue("@userId", messageDTO.UserId);
            cmd.Parameters.AddWithValue("@groupId", messageDTO.GroupId);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                // TODO: this code should never trigger, it is here in case the database doesnt have the last insert id, but it should because we just inserted something, i need to check with teacher if this should stay
                conn.Close();
                return messageDTO;
            }
            messageDTO.SetMessageId(reader.GetInt32(0));
            conn.Close();
            return messageDTO;
        }

        public bool DeleteMessage(int messageId)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"DELETE FROM messages WHERE id = @messageId", conn
            );
            cmd.Parameters.AddWithValue("@messageId", messageId);
            cmd.ExecuteNonQuery();
            conn.Close();
            return true;
        }

        public bool EditMessage(MessageDTO messageDTO, string newText)
        {

            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"UPDATE messages SET text = @bodyText WHERE id = @id", conn
            );
            cmd.Parameters.AddWithValue("@id", messageDTO.MessageId);
            cmd.Parameters.AddWithValue("@bodyText", newText);
            cmd.ExecuteNonQuery();
            conn.Close();
            messageDTO.EditText(newText);
            return true;
        }
    }
}