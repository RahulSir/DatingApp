using System;

namespace DatingApp.API.DTOs
{
    public class UserForListDTO
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public int  age { get; set; }
        public  string knownAS { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }      

        public string City { get; set; }

        public string Country {get;set;}

        public string photourl{get;set;}

        
    }
}