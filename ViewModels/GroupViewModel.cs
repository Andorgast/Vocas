using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Bcpg;
using System.Data.SqlTypes;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Vocas.ViewModels
{
    public class GroupViewModel
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";
        public enum UserAdding
        {
            user_not_found, 
            group_is_full, 
            success
        }
        public enum GroupAdding
        {
            failed_get_groupid,
            success
        }
        public int GroupId { get; private set; }
        public int MaxGroupSize { get; private set; } = 4;
        public List<UserViewModel> Users { get; private set; } = [];

        public GroupViewModel(int groupId)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM user_to_group WHERE group_id=@id", conn
            );
            cmd.Parameters.AddWithValue("@id", groupId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Users.Add(new UserViewModel(reader.GetInt32(1)));
            }
            GroupId = groupId;
            conn.Close();
        }

        public GroupViewModel(UserViewModel[] users)
        {
            if((users.Length + Users.Count()) < MaxGroupSize)
            {
                foreach (var user in users)
                {
                    Users.Add(user);
                }
                AddGroupToDb();
            }
        }

        public UserAdding AddUserToGroup(string username)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT id FROM users WHERE username = @username", conn
            );
            cmd.Parameters.AddWithValue("@username", username);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return UserAdding.user_not_found;
            }
            int userId = reader.GetInt32(0);
            conn.Close();
            if (Users.Count() > (MaxGroupSize - 1))
            {
                return UserAdding.group_is_full;
            }
            conn.Open();
            cmd = new MySqlCommand(
                @"INSERT INTO user_to_group (user_id, group_id) VALUE (@userId, @groupId)", conn
            );
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@groupId", GroupId);
            cmd.ExecuteNonQuery();
            conn.Close();
            return UserAdding.success;
        }

        public GroupAdding AddGroupToDb()
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"INSERT INTO game_groups (name) VALUE (@name)", conn    
            );
            cmd.Parameters.AddWithValue("@name", "name of the group");
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Open();
            cmd = new MySqlCommand(
                @"SELECT LAST_INSERT_ID();", conn
            );
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("FAILED TO GET LAST ID");
                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("-------------------------------------------------------------------");
                return GroupAdding.failed_get_groupid;
            }
            GroupId = reader.GetInt32(0);
            conn.Close();
            foreach (var user in Users)
            {
                conn.Open();
                cmd = new MySqlCommand(
                    @"INSERT INTO user_to_group (user_id, group_id) VALUE (@userId, @groupId)", conn
                );
                cmd.Parameters.AddWithValue("@userId", user.UserId);
                cmd.Parameters.AddWithValue("@groupId", GroupId);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return GroupAdding.success;
        }
    }
}
