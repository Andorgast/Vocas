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
        public List<MessageViewModel> Messages = new();
        public int? Editing { get; private set; }
        public IActionResult OnGet(int? id, string? newUser, int? votedUser, int? votingUser, int? editing, string? editedMessage, string? newMessage)
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

                var conn = new MySqlConnection(connectionString);
                conn.Open();
                var cmd = new MySqlCommand(
                    @"SELECT id FROM messages WHERE group_id=@groupId", conn
                );
                cmd.Parameters.AddWithValue("@groupId", GroupInfo.GroupId);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Messages.Add(new MessageViewModel(reader.GetInt32(0)));
                }
                conn.Close();

                if (newMessage != null)
                {
                    Messages.Add(new MessageViewModel(newMessage, currentUserId, GroupInfo.GroupId));
                    return RedirectToPage("/Group", new { id = GroupInfo.GroupId });
                }

                if(editing != null && editedMessage != null)
                {
                    foreach (var message in Messages)
                    {
                        if(message.Id == editing)
                        {
                            message.EditMessage(currentUserId, editedMessage);
                            return RedirectToPage("/Group", new { id = GroupInfo.GroupId });
                        }
                    }
                }
                else if(editing != null)
                {
                    Editing = editing;
                }

                if(newUser != null)
                {
                    GroupInfo.AddUserToGroup(newUser);
                    return RedirectToPage("/Group", new { id = GroupInfo.GroupId });
                }
                
                if (votedUser != null && votingUser != null)
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
                    @"SELECT group_id FROM user_to_group WHERE user_id=@id", conn
                );
                cmd.Parameters.AddWithValue("@id", currentUserId);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserGroups.Add(new GroupViewModel(reader.GetInt32(0)));
                }
                conn.Close();
            }
            return Page();
        }
    }
}
