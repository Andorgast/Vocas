namespace presentation_layer.Models
{
    public class UserListViewModel
    {
        public List<UserViewModel> Users { get; private set; } = [];

        public UserListViewModel(List<UserViewModel> users)
        {
            Users = users;
        }
    }
}
