namespace presentation_layer.Models
{
    public class SignUpViewModel
    {
        public bool UsernameAlreadyTaken = false;
        public bool NoNameEntered = false;
        public bool PasswordWrong = false;
        public string? Password;
        public string? Username;

        public void NameNotEntered(string? password)
        {
            NoNameEntered = true;
            Password = password;
        }

        public void PasswordNotComplying(string? name)
        {
            PasswordWrong = true;
            Username = name;
        }

        public void UsernameInUse(string name, string password)
        {
            Username = name;
            Password = password;
            UsernameAlreadyTaken = true;
        }
    }
}
