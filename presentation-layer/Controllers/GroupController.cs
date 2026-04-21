using logic_layer;
using Microsoft.AspNetCore.Mvc;
using presentation_layer.Models;
using System.Diagnostics;

namespace presentation_layer.Controllers
{
    public class GroupController : Controller
    {
        private GroupService GroupService = new();
        private UserService UserService = new();
        private MessageService MessageService = new();
        private int CurrentUser = 7;

        public IActionResult Index()
        {
            List<GroupModel> groupModelList = GroupService.GetAllGroupsByUser(CurrentUser);
            List<GroupViewModel> groupList = [];
            foreach (GroupModel group in groupModelList)
            {
                List<UserViewModel> userList = [];
                foreach (int userId in group.UserIds)
                {
                    //in the current case userModel can never return null
                    UserModel userModel = UserService.GetUserById(userId);
                    userList.Add(new(userModel.UserId, userModel.Username, userModel.GetKD(), userModel.TeamKills, userModel.Playtime, userModel.FavoredFactions));
                }
                groupList.Add(new(group.GroupId, userList));
            }
            return View(new GroupIndexViewModel(CurrentUser, groupList));
        }

        public IActionResult GroupPage(int id, string? newUser, int? votedUser, int? votingUser, int? editing, string? editedMessage, string? newMessage)
        {
            GroupModel? groupModel = GroupService.GetGroupById(id, CurrentUser);
            if (groupModel == null)
            {
                return RedirectToAction("Index", "Group");
            }
            List<UserViewModel> userList = [];
            foreach(int userId in groupModel.UserIds)
            {
                //userModel is never null here
                UserModel userModel = UserService.GetUserById(userId);
                userList.Add(new(userModel.UserId, userModel.Username, userModel.GetKD(), userModel.TeamKills, userModel.Playtime, userModel.FavoredFactions));
            }
            GroupViewModel group = new(groupModel.GroupId, userList);

            MessageService messageService = new();
            messageService.GetAllMessagesByGroup(id);
            List<MessageViewModel> messageList = [];
            foreach (MessageModel message in messageService.MessageModelList)
            {
                var UserViewModel = new UserViewModel(message.User.UserId, message.User.Username, message.User.GetKD(), message.User.TeamKills, message.User.Playtime, message.User.FavoredFactions);
                messageList.Add(new(message.MessageId, message.BodyText, UserViewModel, message.GroupId));
            }

            if (editing != null && editedMessage != null)
            {
                messageService.EditMessage(new((int)editing, editedMessage, CurrentUser, group.GroupId));
                return RedirectToAction("GroupPage", "Home");
            }
            else if (newMessage != null)
            {
                messageService.SendMessage(new(newMessage, CurrentUser, group.GroupId));
                return RedirectToAction("GroupPage", "Home");
            }

            if (groupService.AddUserToGroup(newUser))
            {
                return RedirectToAction("GroupPage", "Home");
            }

            if (groupService.VoteForRemoval(votingUser, votedUser) != GroupService.UserRemoving.null_value)
            {
                return RedirectToAction("GroupPage", "Home");
            }
            messageService.EditMessage(CurrentUser, editedMessage, editing);

            return View(new GroupPageViewModel(CurrentUser, groupInfo, groupService.GroupModel.Users.Count(), messageList, editing));
        }
    }
}
