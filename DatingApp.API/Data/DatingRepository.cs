using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Photo> GetMainPhoto(int userid)
        {
            return await _context.Photos.FirstOrDefaultAsync(p =>p.isMain && p.UserId==userid);
            //return await _context.Photos.Where(u=>u.UserId == userid).FirstOrDefaultAsync(p=>p.isMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.id == id);
            return photo ;
        }

        public async Task<User> GetUser(int id)
        {
            var user =  await _context.Users.Include(p=>p.Photos).FirstOrDefaultAsync(x=>x.id == id);
            return user ; 
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(x => x.Photos).ToListAsync();
            return users ;
        }

        // This function ensures if there is some change happend to db
        // or not either if their is nothing to change otr some problem occured while 
        // changing the db
        //If any change occurs it will gie some value greater than zero
        // else the value will be zero
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync()>0;
        }
    }
}