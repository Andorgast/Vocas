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
                GroupCount = GroupInfo.Users.Count();
            }
            else
            {
                GroupCount = 0;
                var conn = new MySqlConnection(connectionString);
                conn.Open();
                var cmd = new MySqlCommand(
                    @"SELECT * FROM user_to_group WHERE user_id=@id", conn
                );
                cmd.Parameters.AddWithValue("@id", currentUserId);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserGroups.Add(new GroupViewModel(reader.GetInt32(2)));
                }
                conn.Close();
            }
        }
    }
}
