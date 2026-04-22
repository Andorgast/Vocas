using logic_layer;
using Microsoft.AspNetCore.Mvc;
using presentation_layer.Models;

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
                    UserModel? userModel = UserService.GetUserById(userId);
                    if (userModel == null)
                    {
                        //there somehow has been a user added into the group that does not exist, remove the user and continue
                        GroupService.RemoveUserFromGroup(userId, group.GroupId);
                    }
                    else
                    {
                        List<AvailabilityModel> availabilityModelList = userModel.Availability;
                        List<AvailabilityViewModel> availabilityViewModelList = [];
                        foreach (AvailabilityModel availability in availabilityModelList)
                        {
                            availabilityViewModelList.Add(new(availability.Id, availability.Day, availability.StartTime, availability.EndTime));
                        }
                        userList.Add(new((int)userModel.UserId, userModel.Username, availabilityViewModelList, userModel.GetKD(), userModel.TeamKills, userModel.Playtime, userModel.FavoredFactions));
                    }
                }
                groupList.Add(new(group.GroupId, userList));
            }
            return View(new GroupIndexViewModel(CurrentUser, groupList));
        }

        public IActionResult GroupPage(int id, string? newUser, int? votedUser, int? editing, string? editedMessage, string? newMessage)
        {
            GroupModel? groupModel = GroupService.GetGroupById(id, CurrentUser);
            if (groupModel == null)
            {
                return RedirectToAction("Index", "Group");
            }

            List<UserViewModel> userList = [];
            foreach (int userId in groupModel.UserIds)
            {
                UserModel? userModel = UserService.GetUserById(userId);
                if (userModel == null)
                {
                    //there somehow has been a user added into the group that does not exist, remove the user and continue
                    GroupService.RemoveUserFromGroup(userId, groupModel.GroupId);
                }
                else
                {
                    List<AvailabilityModel> availabilityModelList = userModel.Availability;
                    List<AvailabilityViewModel> availabilityViewModelList = [];
                    foreach (AvailabilityModel availability in availabilityModelList)
                    {
                        availabilityViewModelList.Add(new(availability.Id, availability.Day, availability.StartTime, availability.EndTime));
                    }
                    userList.Add(new((int)userModel.UserId, userModel.Username, availabilityViewModelList, userModel.GetKD(), userModel.TeamKills, userModel.Playtime, userModel.FavoredFactions));
                }
            }
            GroupViewModel group = new(groupModel.GroupId, userList);

            List<MessageModel> messageModelList = MessageService.GetAllMessagesByGroup(id);
            List<MessageViewModel> messageList = [];
            foreach (MessageModel message in messageModelList)
            {
                UserModel? userModel = UserService.GetUserById(message.UserId);
                if (userModel == null)
                {
                    //there somehow has been sent a message by a user that doesnt exist, remove the erroredUsers message and continue
                    MessageService.DeleteMessage(message.UserId, message.MessageId);
                }
                else
                {
                    List<AvailabilityModel> availabilityModelList = userModel.Availability;
                    List<AvailabilityViewModel> availabilityViewModelList = [];
                    foreach (AvailabilityModel availability in availabilityModelList)
                    {
                        availabilityViewModelList.Add(new(availability.Id, availability.Day, availability.StartTime, availability.EndTime));
                    }
                    messageList.Add(new(message.MessageId, message.BodyText, new((int)userModel.UserId, userModel.Username, availabilityViewModelList, userModel.GetKD(), userModel.TeamKills, userModel.Playtime, userModel.FavoredFactions), message.GroupId));
                }
            }

            if (newMessage != null)
            {
                MessageService.SendMessage(new(newMessage, CurrentUser, group.GroupId));
                return RedirectToAction("GroupPage", "Group");
            }

            if (newUser != null)
            {
                GroupService.AddUserToGroup(newUser, groupModel);
                return RedirectToAction("GroupPage", "Group");
            }

            if (votedUser != null)
            {
                GroupService.VoteForRemoval(CurrentUser, (int)votedUser, groupModel);
                return RedirectToAction("GroupPage", "Group");
            }

            if (editing != null && editedMessage != null)
            {
                MessageService.EditMessage(new((int)editing, editedMessage, CurrentUser, group.GroupId));
                return RedirectToAction("GroupPage", "Group");
            }

            return View(new GroupPageViewModel(CurrentUser, group, group.Users.Count(), messageList, editing));
        }
    }
}
