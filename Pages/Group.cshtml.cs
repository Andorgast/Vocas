using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data.SqlTypes;
using Vocas.ViewModels;

namespace Vocas.Pages
{
    public class GroupModel : PageModel
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";
        public int currentUserId { get; private set; } = 7;
        public GroupViewModel? GroupInfo { get; private set; }
        public int GroupCount { get; private set; }
        public List<GroupViewModel> UserGroups = new();
        public void OnGet(int? id, string? newUser)
        {
            if(id != null)
            {
                GroupInfo = new GroupViewModel((int)id);
                if(newUser != null)
                {
                    GroupInfo.AddUserToGroup(newUser);
                }
                GroupCount = 2;
                if (GroupInfo.User3 != null)
                {
                    if (GroupInfo.User4 != null)
                    {
                        GroupCount = 4;
                    }
                    else
                    {
                        GroupCount = 3;
                    }
                }
            }
            else
            {
                GroupCount = 0;
                var conn = new MySqlConnection(connectionString);
                conn.Open();
                var cmd = new MySqlCommand(
                    @"SELECT * FROM game_groups WHERE user1_id=@id OR user2_id=@id OR user3_id=@id OR user4_id=@id", conn
                );
                cmd.Parameters.AddWithValue("@id", currentUserId);
                var reader = cmd.ExecuteReader();
                if (reader.IsClosed)
                {
                    conn.Close();
                    return;
                }
                while (reader.Read())
                {
                    try
                    {
                        UserGroups.Add(new GroupViewModel(reader.GetInt32(0), new UserViewModel(reader.GetInt32(1)), new UserViewModel(reader.GetInt32(2)), new UserViewModel(reader.GetInt32(3)), new UserViewModel(reader.GetInt32(4))));
                    }
                    catch (SqlNullValueException)
                    {
                        try
                        {
                            UserGroups.Add(new GroupViewModel(reader.GetInt32(0), new UserViewModel(reader.GetInt32(1)), new UserViewModel(reader.GetInt32(2)), new UserViewModel(reader.GetInt32(3)), null));
                        }
                        catch (SqlNullValueException)
                        {
                            UserGroups.Add(new GroupViewModel(reader.GetInt32(0), new UserViewModel(reader.GetInt32(1)), new UserViewModel(reader.GetInt32(2)), null, null));
                        }
                    }
                    
                }
                conn.Close();
            }
        }
    }
}
