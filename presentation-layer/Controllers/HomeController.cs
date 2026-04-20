using logic_layer;
using Microsoft.AspNetCore.Mvc;
using presentation_layer.Models;
using System.Diagnostics;

namespace presentation_layer.Controllers
{
    public class HomeController : Controller
    {
        private int CurrentUser = 7;

        public IActionResult UserList(int? groupCreate)
        {
            if (groupCreate != null)
            {
                GroupService groupService = new();
                groupService.CreateNewGroup([(int)groupCreate]);
                return GroupPage(groupService.GroupModel.GroupId, null, null, null, null, null, null);
            }
            else
            {
                UserService userService = new();
                List<UserViewModel> userList = [];
                foreach (UserModel user in userService.UserModeList)
                {
                    userList.Add(new(user.UserId, user.Username, user.GetKD(), user.TeamKills, user.Playtime, user.FavoredFactions));
                }
                return View(new UserListViewModel(userList));
            }
        }

        public IActionResult GroupPage(int? id, string? newUser, int? votedUser, int? votingUser, int? editing, string? editedMessage, string? newMessage)
        {
            GroupService groupService = new();
            if (id != null)
            {
                groupService.GetGroupById((int)id);
                List<UserViewModel> userList = [];
                foreach (UserModel user in groupService.GroupModel.Users)
                {
                    userList.Add(new(user.UserId, user.Username, user.GetKD(), user.TeamKills, user.Playtime, user.FavoredFactions));
                }
                GroupViewModel groupInfo = new(groupService.GroupModel.GroupId, userList);

                MessageService messageService = new();
                messageService.GetAllMessagesByGroup((int)id);
                List<MessageViewModel> messageList = [];
                foreach (MessageModel message in messageService.MessageModelList)
                {
                    var UserViewModel = new UserViewModel(message.User.UserId, message.User.Username, message.User.GetKD(), message.User.TeamKills, message.User.Playtime, message.User.FavoredFactions);
                    messageList.Add(new(message.MessageId, message.BodyText, UserViewModel, message.GroupId));
                }

                if(messageService.SendMessage(newMessage, groupService.GroupModel.GroupId, CurrentUser))
                {
                    newMessage = null;
                    return RedirectToPage("/GroupPage");
                }
                messageService.EditMessage(CurrentUser, editedMessage, editing);

                return View(new GroupPageViewModel(CurrentUser, groupInfo, groupService.GroupModel.Users.Count(), messageList, editing));
            }
            else
            {
                groupService.GetAllGroupsByUser(CurrentUser);
                List<GroupViewModel> groupList = [];
                foreach (GroupModel group in groupService.GroupModelList)
                {
                    List<UserViewModel> userList = [];
                    foreach (UserModel user in groupService.GroupModel.Users)
                    {
                        userList.Add(new(user.UserId, user.Username, user.GetKD(), user.TeamKills, user.Playtime, user.FavoredFactions));
                    }
                    groupList.Add(new(group.GroupId, userList));
                }
                if (groupList.Count <= 0)
                {
                    return Index();
                }
                return View(new GroupPageViewModel(CurrentUser, groupList));
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}