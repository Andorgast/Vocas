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
        public List<UserViewModel> Users { get; private set; } = [];
        public async Task<IActionResult> OnGet()
        {
            await using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();
            await using var cmd = new MySqlCommand(
                @"SELECT * FROM users", conn
            );
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) 
            {
                Users.Add(new UserViewModel(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetString(6), [reader.GetString(7)]));
            }
            conn.Close();
            return Page();
        }
    }
}
