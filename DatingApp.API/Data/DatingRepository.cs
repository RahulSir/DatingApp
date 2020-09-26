using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recepientId)
        {
            return await _context.Likes
            .FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recepientId);
        }

        public async Task<Photo> GetMainPhoto(int userid)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.isMain && p.UserId == userid);
            //return await _context.Photos.Where(u=>u.UserId == userid).FirstOrDefaultAsync(p=>p.isMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.id == id);
            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userparams)
        {
            var users = _context.Users.Include(x => x.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.id != userparams.UserId);

            users = users.Where(u => u.Gender == userparams.Gender);
            if (userparams.Likers)
            {
                var userLikers = await GetUserLikes(userparams.UserId, userparams.Likers);
                users = users.Where(u => userLikers.Contains(u.id));
            }
            if (userparams.Likees)
            {
                var userLikees = await GetUserLikes(userparams.UserId, userparams.Likers);
                users = users.Where(u => userLikees.Contains(u.id));
            }
            if (userparams.minAge != 18 || userparams.maxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userparams.maxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userparams.minAge);
                users = users.Where(u => u.DateofBirth <= maxDob && u.DateofBirth >= minDob);
            }
            if (!string.IsNullOrEmpty(userparams.OrderBy))
            {
                switch (userparams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }
            return await PagedList<User>.CreateAsync(users, userparams.PageSize, userparams.PageNumber);
        }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var users = await _context.Users
                        .Include(u => u.Likers)
                        .Include(u => u.Likees)
                        .FirstOrDefaultAsync(u => u.id == id);
            if (likers)
            {
                return  users.Likers.Where(u => u.LikeeId == id).Select(i => i.LikerId);
            }
            else
            {
                return  users.Likees.Where(u => u.LikerId == id).Select(i => i.LikeeId);
            }
        }

        // This function ensures if there is some change happend to db
        // or not either if their is nothing to change otr some problem occured while 
        // changing the db
        //If any change occurs it will gie some value greater than zero
        // else the value will be zero
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages
                        .Include(s => s.Sender)
                        .ThenInclude(p => p.Photos)
                        .Include(r => r.Recepient)
                        .ThenInclude(p=>p.Photos).AsQueryable();
            switch(messageParams.MessageContainer){
                case "Inbox":
                messages = messages.Where(u => u.RecepientId == messageParams.UserId 
                            && u.RecepientDeleted ==false);
                break;
                case "Outbox":
                messages = messages.Where(u => u.SenderId == messageParams.UserId
                            && u.SenderDeleted == false);
                break;
                default:
                messages = messages.Where(u => u.RecepientId == messageParams.UserId 
                            && u.RecepientDeleted ==false && u.isRead ==false);
                break;

            }
            messages = messages.OrderByDescending(d => d.MessageSent);
            return await PagedList<Message>.CreateAsync(messages , messageParams.PageSize, messageParams.PageNumber);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recepientId)
        {
            var messages = await _context.Messages
                        .Include(s => s.Sender)
                        .ThenInclude(p => p.Photos)
                        .Include(r => r.Recepient)
                        .ThenInclude(p=>p.Photos)
                        .Where(m => m.SenderId == userId && m.RecepientId == recepientId
                        && m.RecepientDeleted == false
                        || m.RecepientId==userId && m.SenderId == recepientId
                        && m.SenderDeleted == false)
                        .OrderByDescending(m => m.MessageSent).ToListAsync();
            return messages;
        }
    }
}