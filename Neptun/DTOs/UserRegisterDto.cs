using Neptun.Models;

namespace Neptun.DTOs
{
    public class UserRegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserType Type { get; set; }
        public StudyMode Mode { get; set; }
    }
}
