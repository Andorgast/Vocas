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
        public int GroupId { get; private set; }
        public UserViewModel User1 { get; private set; }
        public UserViewModel User2 { get; private set; }
        public UserViewModel? User3 { get; private set; }
        public UserViewModel? User4 { get; private set; }

        public GroupViewModel(int groupId)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM game_groups WHERE id=@id", conn
            );
            cmd.Parameters.AddWithValue("@id", groupId);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return;
            }
            GroupId = groupId;
            User1 = new UserViewModel(reader.GetInt32(1));
            User2 = new UserViewModel(reader.GetInt32(2));
            try
            {
                User4 = new UserViewModel(reader.GetInt32(4));
                User3 = new UserViewModel(reader.GetInt32(3));
            }
            catch (SqlNullValueException)
            {
                try
                {
                    User3 = new UserViewModel(reader.GetInt32(3));
                }
                catch (SqlNullValueException)
                {
                }
            }
            conn.Close();
        }

        public GroupViewModel(UserViewModel user1, UserViewModel user2)
        {
            User1 = user1;
            User2 = user2;
            AddGroupToDb();
            return;
        }

        public GroupViewModel(int groupId, UserViewModel user1, UserViewModel user2, UserViewModel? user3, UserViewModel? user4)
        {
            GroupId = groupId;
            User1 = user1;
            User2 = user2;
            User3 = user3;
            User4 = user4;
            return;
        }

        public void AddUserToGroup(string username)
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
                return;
            }
            int userId = reader.GetInt32(0);
            conn.Close();
            if(userId == User1.UserId || userId == User2.UserId)
            {
                return;
            }
            conn.Open();
            if (User3 == null)
            {
                cmd = new MySqlCommand(
                    @" UPDATE game_groups SET user3_id = @userId WHERE id = @groupId", conn
                );
                User3 = new UserViewModel(userId);
            }
            else if(User4 == null)
            {
                if (userId == User3.UserId)
                {
                    return;
                }
                cmd = new MySqlCommand(
                    @" UPDATE game_groups SET user4_id = @userId WHERE id = @groupId", conn
                );
                User4 = new UserViewModel(userId);
            }
            else
            {
                return;
            }
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@groupId", GroupId);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void AddGroupToDb()
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand();
            if (User4 != null)
            {
                cmd = new MySqlCommand(
                    @"INSERT INTO game_groups (user1_id, user2_id, user3_id, user4_id) VALUE (@user1Id, @user2Id, @user3Id, @user4Id) ", conn
                );
                cmd.Parameters.AddWithValue("@user3Id", User3.UserId);
                cmd.Parameters.AddWithValue("@user4Id", User4.UserId);

            }
            else if (User3 != null)
            {
                cmd = new MySqlCommand(
                    @"INSERT INTO game_groups (user1_id, user2_id, user3_id) VALUE (@user1Id, @user2Id, @user3Id) ", conn
                );
                cmd.Parameters.AddWithValue("@user3_id", User3.UserId);
            }
            else
            {
                cmd = new MySqlCommand(
                    @"INSERT INTO game_groups (user1_id, user2_id) VALUE (@user1Id, @user2Id) ", conn
                );
            }
            cmd.Parameters.AddWithValue("@user1Id", User1.UserId);
            cmd.Parameters.AddWithValue("@user2Id", User2.UserId);
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Open();
            cmd = new MySqlCommand(
                @"SELECT LAST_INSERT_ID();", conn
            );
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return;
            }
            GroupId = reader.GetInt32(0);
            conn.Close();
        }
    }
}
