using System.ComponentModel.DataAnnotations;
using static SocialApp.infrastructure.Data.Constants.DataConstants;

namespace SocialAppAPI.Models.User
{
    public class UserRegisterDto
    {
        [Required]
        [MaxLength(UsernameMaxLength)]
        [MinLength(UsernameMinLength)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(PasswordMaxLength)]
        [MinLength(PasswordMinLength)]
        public string Password { get; set; } = null!;
    }
}
