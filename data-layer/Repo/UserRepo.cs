using MySql.Data.MySqlClient;
namespace data_layer
{
    public class UserRepo
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";

        public UserDTO? GetUserById(int userId)
        {
            UserDTO? userToReturn = null;
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM users WHERE id=@id", conn
            );
            cmd.Parameters.AddWithValue("@id", userId);
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                conn.Close();
                return userToReturn;
            }
            userToReturn = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetString(6), reader.GetString(7));
            return userToReturn;
        }

        public UserDTO? GetUserByName(string username)
        {
            UserDTO? userToReturn = null;
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM users WHERE username=@username", conn
            );
            cmd.Parameters.AddWithValue("@username", username);
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                conn.Close();
                return userToReturn;
            }
            userToReturn = new UserDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetString(6), reader.GetString(7));
            return userToReturn;
        }

        public List<UserDTO> GetAllUsers()
        {
            List<UserDTO> AllUsers = [];
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM users", conn
            );
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AllUsers.Add(new UserDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetString(6), reader.GetString(7)));
            }
            return AllUsers;
        }
    }
}
