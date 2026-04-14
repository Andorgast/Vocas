using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data.SqlTypes;
using Vocas.ViewModels;
using static Vocas.ViewModels.GroupViewModel;

namespace Vocas.Pages
{
    public class GroupModel : PageModel
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";
        public int currentUserId { get; private set; } = 7;
        public GroupViewModel? GroupInfo { get; private set; }
        public int GroupCount { get; private set; }
        public List<GroupViewModel> UserGroups = new();
        public IActionResult OnGet(int? id, string? newUser, int? votedUser, int? votingUser)
        {
            if(id != null)
            {
                GroupInfo = new GroupViewModel((int)id);
                bool isInGroup = false;
                foreach(var user in GroupInfo.Users)
                {
                    if (!isInGroup && user.UserId == currentUserId)
                    {
                        isInGroup = true;
                    }
                }
                if (!isInGroup)
                {
                    return RedirectToPage("/Group");
                }
                if(newUser != null)
                {
                    GroupInfo.AddUserToGroup(newUser);
                    return RedirectToPage("/Group", new { id = GroupInfo.GroupId });
                }
                else if (votedUser != null && votingUser != null)
                {
                    GroupInfo.VoteForRemoval((int)votingUser, (int)votedUser);
                    return RedirectToPage("/Group", new { id = GroupInfo.GroupId });
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
            return Page();
        }
    }
}
