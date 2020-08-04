using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserLoginDTO
    {

        [Required]
        public string username { get; set; }

        [Required]
        public string password{get;set;}
    }
}