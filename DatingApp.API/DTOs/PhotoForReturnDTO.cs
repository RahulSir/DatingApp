using System;

namespace DatingApp.API.DTOs
{
    public class PhotoForReturnDTO
    {
         public int id { get; set; }
        public string Url { get; set; }

        public string Description {get; set;}

        public bool isMain { get; set; }

        public DateTime DateAdded { get; set; }

        public string PublicId{get;set;}
    }
}