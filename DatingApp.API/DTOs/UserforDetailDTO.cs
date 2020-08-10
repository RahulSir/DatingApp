using System;
using System.Collections.Generic;
using DatingApp.API.Models;

namespace DatingApp.API.DTOs
{
    public class UserforDetailDTO
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public int age { get; set; }
        public  string knownAS { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction {get; set;}

        public string LookingFor { get; set; }

        public string City { get; set; }

        public string Country {get;set;}

        public string photourl {get;set;}

        public ICollection<Photo> Photos{get;set;}

    }
}