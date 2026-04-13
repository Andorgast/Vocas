using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Vocas.ViewModels;

namespace Vocas.Pages
{
    public class UserListModel : PageModel
    {
        private string connectionString = "Server=localhost;Database=s2proj;User Id=root;Password=1234;";
        public decimal GetKd(decimal kills, decimal deaths)
        {
            decimal KD;
            if(deaths > 0)
            {
                KD = Math.Round((kills / deaths), 1);
            }
            else
            {
                KD = 1;
            }
            return KD;
        }
        public void createGroup(int UserToAdd)
        {
            var CreatedGroup = new GroupViewModel([new UserViewModel(UserToAdd), new UserViewModel(currentUserId)]);
            RedirectToPage("/group", new { id = CreatedGroup.GroupId});
        }
        public int currentUserId { get; private set; } = 7;
        public List<UserViewModel> Users { get; private set; } = [];
        public PageResult OnGet(int? groupCreate)
        {
            if (groupCreate != null)
            {
                if ((int)groupCreate != currentUserId)
                {
                    createGroup((int)groupCreate);
                }    
            }
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                @"SELECT * FROM users WHERE NOT id=@id", conn
            );
            cmd.Parameters.AddWithValue("@id", currentUserId);
            var reader = cmd.ExecuteReader();
            if (reader.IsClosed)
            {
                conn.Close();
                return Page();
            }
            while (reader.Read()) 
            {
                Users.Add(new UserViewModel(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetString(6), reader.GetString(7).Replace("\n","")));
            }
            conn.Close();
            return Page();
        }
    }
}
