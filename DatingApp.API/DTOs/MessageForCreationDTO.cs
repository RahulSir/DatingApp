using System;

namespace DatingApp.API.DTOs
{
    public class MessageForCreationDTO
    {
        public int SenderId { get; set; }
        public int RecepientId { get; set; }
        public string Content { get; set; }

        public DateTime MessageSent{get;set;}

        public MessageForCreationDTO()
        {
            MessageSent = DateTime.Now;
            
        }
    }
}