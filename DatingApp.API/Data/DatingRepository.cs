using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Models;
using System.Collections.Generic;

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

        public async Task<user> GetUser(int id)
        {
            var User = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id ==id);

            return User;
        }

        public async Task<IEnumerable<user>> GetUsers()
        {
            var users = await _context.Users.Include(p=> p.Photos).ToListAsync();    
            return users;    
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync()>0;
        }
    }
}