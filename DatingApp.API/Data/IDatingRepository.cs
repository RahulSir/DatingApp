using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        // Here we are creating a Add generic function for adding User or Photo
        // T is type of class only
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userparams);
         Task<User> GetUser(int id);
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhoto(int userid);
         Task<Like> GetLike(int userId , int recepientId);
         Task<Message> GetMessage(int id);
         
         //For inbox and outbox of the user
         Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);

         // For getting conversation between 2 users......
         Task<IEnumerable<Message>> GetMessageThread(int userId , int recepientId);

         


    }
}