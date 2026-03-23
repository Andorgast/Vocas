using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vocas.ViewModels;

namespace Vocas.Pages
{
    public class GroupModel : PageModel
    {
        public GroupViewModel GroupInfo { get; private set; }
        public int GroupCount { get; private set; }
        public void OnGet(int id)
        {
            GroupInfo = new GroupViewModel(id);
            if(GroupInfo.User3 != null)
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
    }
}
