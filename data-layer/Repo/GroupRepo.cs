using MySql.Data.MySqlClient;
namespace data_layer
{
    public class GroupRepo
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";

        public List<GroupDTO> GetAllGroupsByUser(int userId)
        {
            var conn = new MySqlConnection(connectionString);
            List<GroupDTO> AllUserGroups = [];
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT group_id FROM user_to_group WHERE user_id=@userId", conn
            );
            cmd.Parameters.AddWithValue("@userId", userId);
            using var reader = cmd.ExecuteReader();
            List<int> groupIdList = [];
            while (reader.Read())
            {
                groupIdList.Add(reader.GetInt32(0));
            }
            conn.Close();

            foreach (int groupId in groupIdList)
            {
                AllUserGroups.Add(GetGroupById(groupId));
            }
            return AllUserGroups;
        }

        public bool CheckIfUserIsInGroup(int groupId, int requestingUser)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM user_to_group WHERE user_id=@requestingUser AND group_id=@groupId", conn
            );
            cmd.Parameters.AddWithValue("@requestingUser", requestingUser);
            cmd.Parameters.AddWithValue("@groupId", groupId);
            using var reader = cmd.ExecuteReader();
            int? TestValue = reader.GetInt32(0);
            conn.Close();
            if (TestValue == null)
            {
                return false;
            }
            return true;
        }

        public GroupDTO GetGroupById(int groupId)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT user_id FROM user_to_group WHERE group_id=@groupId", conn
            );
            cmd.Parameters.AddWithValue("@groupId", groupId);
            using var reader = cmd.ExecuteReader();
            List<int> tempList = [];
            while (reader.Read())
            {
                tempList.Add(reader.GetInt32(0));
            }
            conn.Close();
            return new GroupDTO(groupId, tempList);
        }

        public void AddUserToGroup(GroupDTO groupDTO, int userId)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"INSERT INTO user_to_group (user_id, group_id) VALUE (@userId, @groupId)", conn
            );
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@groupId", groupDTO.GroupId);
            cmd.ExecuteNonQuery();
            conn.Close();
            groupDTO.Users.Add(userId);
        }

        public List<int> GetRemovalVotes(int votingUser, int votedUser, int groupId)
        {
            List<int> votingUsers = [];
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT voting_user FROM removal_vote WHERE voted_user = @votedUser AND group_id = @groupId", conn
            );
            cmd.Parameters.AddWithValue("@votedUser", votedUser);
            cmd.Parameters.AddWithValue("@groupId", groupId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                votingUsers.Add(reader.GetInt32(0));
            }
            conn.Close();
            return votingUsers;
        }

        public void RemoveUserFromGroup(int votedUser, int groupId)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"DELETE FROM user_to_group WHERE user_id = @votedUser AND group_id = @groupId; DELETE FROM removal_vote WHERE voting_user = @votedUser OR (voted_user = @votedUser AND group_id = @groupId)", conn
            );
            cmd.Parameters.AddWithValue("@votedUser", votedUser);
            cmd.Parameters.AddWithValue("@groupId", groupId);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void AddVoteForRenoval(int votingUser, int votedUser, int groupId)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"INSERT INTO removal_vote (voting_user, voted_user, group_id) VALUE (@votingUser, @votedUser, @groupId)", conn
            );
            cmd.Parameters.AddWithValue("@votingUser", votingUser);
            cmd.Parameters.AddWithValue("@votedUser", votedUser);
            cmd.Parameters.AddWithValue("@groupId", groupId);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public GroupDTO AddGroupToDb(GroupDTO groupDTO)
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"INSERT INTO game_groups (name) VALUE (@name); SELECT LAST_INSERT_ID();", conn
            );
            cmd.Parameters.AddWithValue("@name", "name of the group");
            using var reader = cmd.ExecuteReader();
            reader.Read();
            groupDTO.GroupId = reader.GetInt32(0);
            conn.Close();
            foreach (var user in groupDTO.Users)
            {
                conn.Open();
                cmd = new MySqlCommand(
                    @"INSERT INTO user_to_group (user_id, group_id) VALUE (@userId, @groupId)", conn
                );
                cmd.Parameters.AddWithValue("@userId", user);
                cmd.Parameters.AddWithValue("@groupId", groupDTO.GroupId);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return groupDTO;
        }
    }
}
