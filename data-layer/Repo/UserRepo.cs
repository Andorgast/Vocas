using MySql.Data.MySqlClient;
namespace data_layer
{
    public class UserRepo
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";

        public bool CheckIfUserExists(int userId)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT id FROM users WHERE id = @userId", conn
            );
            cmd.Parameters.AddWithValue("@userId", userId);
            var reader = cmd.ExecuteReader();
            reader.Read();
            return reader.IsDBNull(0);
        }

        public UserDTO CreateNewUser(UserDTO userInfo)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"INSERT INTO users (username, password) VALUE (@username, @password); SELECT LAST_INSERT_ID();", conn
            );
            cmd.Parameters.AddWithValue("@username", userInfo.Username);
            cmd.Parameters.AddWithValue("@password", userInfo.Password);
            var reader = cmd.ExecuteReader();
            reader.Read();
            userInfo.UserId = reader.GetInt32(0);
            return userInfo;
        }

        public UserDTO? GetUserById(int userId)
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
                return null;
            }
            int idTemp = reader.GetInt32(0);
            string nameTemp = reader.GetString(1);
            string passwordTemp = reader.GetString(2);
            int? killsTemp = null;
            if (!reader.IsDBNull(3))
            {
                killsTemp = reader.GetInt32(3);
            }
            int? deathsTemp = null;
            if (!reader.IsDBNull(4))
            {
                deathsTemp = reader.GetInt32(4);
            }
            int? teamkillsTemp = null;
            if (!reader.IsDBNull(5))
            {
                teamkillsTemp = reader.GetInt32(5);
            }
            TimeSpan? playtimeTemp = null;
            if (!reader.IsDBNull(6))
            {
                playtimeTemp = TimeSpan.Parse(reader.GetString(6));
            }
            string? factionsTemp = null;
            if (!reader.IsDBNull(7))
            {
                factionsTemp = reader.GetString(7);
            }
            return new(idTemp, nameTemp, passwordTemp, killsTemp, deathsTemp, teamkillsTemp, playtimeTemp, factionsTemp);
        }

        public UserDTO? GetUserByName(string username)
        {
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
                return null;
            }
            int idTemp = reader.GetInt32(0);
            string nameTemp = reader.GetString(1);
            string passwordTemp = reader.GetString(2);
            int? killsTemp = null;
            if (!reader.IsDBNull(3))
            {
                killsTemp = reader.GetInt32(3);
            }
            int? deathsTemp = null;
            if (!reader.IsDBNull(4))
            {
                deathsTemp = reader.GetInt32(4);
            }
            int? teamkillsTemp = null;
            if (!reader.IsDBNull(5))
            {
                teamkillsTemp = reader.GetInt32(5);
            }
            TimeSpan? playtimeTemp = null;
            if (!reader.IsDBNull(6))
            {
                playtimeTemp = TimeSpan.Parse(reader.GetString(6));
            }
            string? factionsTemp = null;
            if (!reader.IsDBNull(7))
            {
                factionsTemp = reader.GetString(7);
            }
            return new(idTemp, nameTemp, passwordTemp, killsTemp, deathsTemp, teamkillsTemp, playtimeTemp, factionsTemp);
        }

        public List<UserDTO> GetAllUsers(int userToExclude)
        {
            List<UserDTO> AllUsers = [];
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM users WHERE NOT id=@userToExclude", conn
            );
            cmd.Parameters.AddWithValue("@userToExclude", userToExclude);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int idTemp = reader.GetInt32(0);
                string nameTemp = reader.GetString(1);
                string passwordTemp = reader.GetString(2);
                int? killsTemp = null;
                if (!reader.IsDBNull(3))
                {
                    killsTemp = reader.GetInt32(3);
                }
                int? deathsTemp = null;
                if (!reader.IsDBNull(4))
                {
                    deathsTemp = reader.GetInt32(4);
                }
                int? teamkillsTemp = null;
                if (!reader.IsDBNull(5))
                {
                    teamkillsTemp = reader.GetInt32(5);
                }
                TimeSpan? playtimeTemp = null;
                if (!reader.IsDBNull(6))
                {
                    playtimeTemp = TimeSpan.Parse(reader.GetString(6));
                }
                string? factionsTemp = null;
                if (!reader.IsDBNull(7))
                {
                    factionsTemp = reader.GetString(7);
                }
                AllUsers.Add(new(idTemp, nameTemp, passwordTemp, killsTemp, deathsTemp, teamkillsTemp, playtimeTemp, factionsTemp));
            }
            return AllUsers;
        }
    }
}
