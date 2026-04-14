using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cms;
using System.Text.RegularExpressions;
using static Vocas.ViewModels.GroupViewModel;
namespace Vocas.ViewModels
{
    public class MessageViewModel
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";
        public int Id { get; private set; }
        public UserViewModel User { get; private set; }
        public int GroupId { get; private set; }
        public string BodyText { get; private set; }
        public DateTime Time { get; private set; }

        public MessageViewModel(int id)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM messages WHERE id=@id", conn
            );
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Id = id;
                User = new UserViewModel(reader.GetInt32(2));
                GroupId = reader.GetInt32(3);
                BodyText = reader.GetString(1);
            }
            if (BodyText == null)
            {
                BodyText = "This message could not be found, it mightve been deleted or we misplaced it.";
            }
            conn.Close();
        }

        public MessageViewModel(string bodyText, int userId, int groupId)
        {
            BodyText = bodyText;
            User = new UserViewModel(userId);
            GroupId = groupId;
            SendMessage();
        }

        public bool SendMessage()
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"INSERT INTO messages (text, user_id, group_id) VALUE (@bodyText, @userId, @groupId); SELECT LAST_INSERT_ID();", conn
            );
            cmd.Parameters.AddWithValue("@bodyText", BodyText);
            cmd.Parameters.AddWithValue("@userId", User.UserId);
            cmd.Parameters.AddWithValue("@groupId", GroupId);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                conn.Close();
                return false;
            }
            Id = reader.GetInt32(0);
            conn.Close();
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
                var conn = new MySqlConnection(connectionString);
                conn.Open();
                var cmd = new MySqlCommand(
                    @"DELETE FROM messages WHERE id = @id", conn
                );
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();
                conn.Close();
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
                BodyText = bodyText;
                var conn = new MySqlConnection(connectionString);
                conn.Open();
                var cmd = new MySqlCommand(
                    @"UPDATE messages SET text = @bodyText WHERE id = @id", conn
                );
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.Parameters.AddWithValue("@bodyText", bodyText);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
        }
    }
}
