using logic_layer;
using Microsoft.AspNetCore.Mvc;
using presentation_layer.Models;

namespace presentation_layer.Controllers
{
    public class UserController : Controller
    {
        private int CurrentUser = 7;
        private GroupService GroupService = new();
        private UserService UserService = new();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserList(int? groupCreate)
        {
            if (groupCreate != null)
            {
                GroupModel groupModel = GroupService.CreateNewGroup([(int)groupCreate, CurrentUser]);
                return RedirectToAction("GroupPage", "Group", new { id = groupModel.GroupId });
            }
            else
            {
                List<UserModel> userModelList = UserService.GetAllUsers(CurrentUser);
                List<UserViewModel> userList = [];
                foreach (UserModel user in userModelList)
                {
                    userList.Add(new(user.UserId, user.Username, user.GetKD(), user.TeamKills, user.Playtime, user.FavoredFactions));
                }
                return View(new UserListViewModel(userList));
            }
        }
    }
}
