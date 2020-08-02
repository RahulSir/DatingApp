using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string password { get; set; }
    }
}