using MySql.Data.MySqlClient;
namespace data_layer
{
    public class UserRepo
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";
        public UserDTO UserDTO { get; private set; }
        public List<UserDTO> UserDTOList { get; private set; } = [];

        public bool GetUserById(int userId)
        {
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
                return false;
            }
            UserDTO = new UserDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetString(6), reader.GetString(7));
            return true;
        }

        public bool GetAllUsers()
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM users", conn
            );
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                UserDTOList.Add(new UserDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetString(6), reader.GetString(7)));
            }
            return true;
        }

        public bool InsertUser()
        {
            return true;
        }
    }
}
