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
            user_already_in_group,
            group_is_full, 
            success
        }
        public enum UserRemoving
        {
            user_not_in_group,
            already_voted,
            voted_succes,
            deleted_succes
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
            foreach(var user in Users)
            {
                if (user.UserId == userId)
                {
                    return UserAdding.user_already_in_group;
                }
            }
            conn.Open();
            cmd = new MySqlCommand(
                @"INSERT INTO user_to_group (user_id, group_id) VALUE (@userId, @groupId)", conn
            );
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@groupId", GroupId);
            cmd.ExecuteNonQuery();
            conn.Close();
            Users.Add(new UserViewModel(userId));
            return UserAdding.success;
        }

        public UserRemoving VoteForRemoval(int votingUser, int votedUser)
        {
            bool votedIsInGroup = false;
            bool votingIsInGroup = false;
            List<int> votingUsers = [];
            foreach (var user in Users)
            {
                if (user.UserId == votedUser)
                {
                    votedIsInGroup = true;
                }
                if (user.UserId == votingUser)
                {
                    votingIsInGroup = true;
                }
            }
            if (!votedIsInGroup || !votingIsInGroup)
            {
                return UserRemoving.user_not_in_group;
            }
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT voting_user FROM removal_vote WHERE voted_user = @votedUser AND group_id = @groupId", conn
            );
            cmd.Parameters.AddWithValue("@votedUser", votedUser);
            cmd.Parameters.AddWithValue("@groupId", GroupId);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                votingUsers.Add(reader.GetInt32(0));
            }
            conn.Close();
            foreach(int userId in votingUsers)
            {
                if(userId == votingUser)
                {
                    return UserRemoving.already_voted;
                }
            }
            votingUsers.Add(votingUser);
            if ((float)votingUsers.Count() > ((float)Users.Count()/2))
            {
                cmd = new MySqlCommand(
                    @"DELETE FROM user_to_group WHERE user_id = @votedUser; DELETE FROM removal_vote WHERE voting_user = @votedUser;"
                );
                cmd.Parameters.AddWithValue("@votedUser", votedUser);
                cmd.ExecuteNonQuery();
                UserViewModel? tempUserContainer = null;
                foreach(var user in Users)
                {
                    if (user.UserId == votedUser) 
                    {
                        tempUserContainer = user;
                    }
                }
                if (tempUserContainer != null)
                {
                    Users.Remove(tempUserContainer);
                }
                else
                {
                    return UserRemoving.user_not_in_group;
                }
                return UserRemoving.deleted_succes;
            }
            return UserRemoving.voted_succes;
        }

        public GroupAdding AddGroupToDb()
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"INSERT INTO game_groups (name) VALUE (@name); SELECT LAST_INSERT_ID();", conn    
            );
            cmd.Parameters.AddWithValue("@name", "name of the group");
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
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
