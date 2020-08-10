using System.Collections.Generic;
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