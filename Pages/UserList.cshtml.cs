using Microsoft.AspNetCore.Mvc.RazorPages;
using Vocas.ViewModels;

namespace Vocas.Pages
{
    public class UserListModel : PageModel
    {
        public decimal GetKd(decimal kills, decimal deaths)
        {
            decimal KD = Math.Round((kills / deaths), 1);
            return KD;
        }
        public List<UserViewModel> Users { get; private set; } = [];
        public void OnGet()
        {
            int i = 0;
            Users.Add(new UserViewModel(i));
            i++;
            while (Users[0].FileRow != null)
            {
                Users.Reverse();
                Users.Add(new UserViewModel(i));
                i++;
                Users.Reverse();
            }
            Users.RemoveAt(0);
            Users.Reverse();
        }
    }
}
