using System.ComponentModel.DataAnnotations;
using System;
namespace DatingApp.API.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        
        public string Username { get; set; }
        [Required]
        [StringLength(20 , MinimumLength =5 , ErrorMessage ="Password must be greater than 5 and less than 20")]
        public string password { get; set; }

         [Required]
        public string knownAs { get; set; }
         [Required]
        public DateTime DateOfBirth { get; set; }
         [Required]
        public string Gender { get; set; }
         [Required]
        public string City { get; set; }
         [Required]
        public string Country { get; set; }
         

        public DateTime Created { get; set; }
         
        public DateTime LastActive { get; set; }
        public UserRegisterDTO()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }


    }
}