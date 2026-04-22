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
                    List<AvailabilityViewModel> availability = [];
                    foreach (AvailabilityModel availabilityModel in user.Availability)
                    {
                        availability.Add(new(availabilityModel.Id, availabilityModel.Day, availabilityModel.StartTime, availabilityModel.EndTime));
                    }
                    userList.Add(new((int)user.UserId, user.Username, availability, user.GetKD(), user.TeamKills, user.Playtime, user.FavoredFactions));
                }
                return View(new UserListViewModel(userList));
            }
        }

        public IActionResult SignUp(string name, string password)
        {
            SignUpViewModel viewModel = new();
            if (password != null)
            {
                if (name != null && UserService.CheckPasswordByCriteria(password))
                {
                    if (UserService.CreateNewUser(new(name, password)) == null)
                    {
                        viewModel.UsernameInUse(name, password);
                        return View(viewModel);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else if (UserService.CheckPasswordByCriteria(password))
                {
                    viewModel.NameNotEntered(password);
                    return View(viewModel);
                }
                else if(password != null && name == null)
                {
                    viewModel.PasswordNotComplying(null);
                    viewModel.NameNotEntered(null);
                    return View(viewModel);
                }
            }
            
            if (name != null)
            {
                viewModel.PasswordNotComplying(name);
                return View(viewModel);
            }
            return View(viewModel);
        }
    }
}
