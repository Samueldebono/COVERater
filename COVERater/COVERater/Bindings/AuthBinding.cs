

namespace COVERater.API.Bindings
{
    public class AuthBinding
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string? Email { get; set; }
        public byte? Experience { get; set; }
        public byte? RoleType { get; set; }
    }
    public class UserExperienceUpdateBinding
    {
        public int UserId { get; set; }
        public byte Experience { get; set; }
    }

    public class ResetPasswordBinding
    { 
        public string Email { get; set; }
    }
 
}
