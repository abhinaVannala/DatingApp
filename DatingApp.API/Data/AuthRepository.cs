using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<user> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username ==username);

            if(user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt )) 
                return null;

            return user;    

            
        }

        private bool VerifyPasswordHash(string password,byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            { 
               
                var computeHash  = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0;i<computeHash.Length;i++)
                {
                    if(computeHash[i]!=passwordHash[i])  return false;
                }
            }
            return true;
        }
        public user User { get;set; }

        public async Task<user> Register(user user, string password)
        {
            this.User = user;
           byte[] passwordHash, passwordSalt;
           CreatePasswordHash(password, out passwordHash, out passwordSalt);
  
           user.PasswordSalt= passwordSalt;
           user.PasswordHash= passwordHash;

           await _context.Users.AddAsync(user);
           await _context.SaveChangesAsync();

           return user;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            { 
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            
            }
        }    

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x=>x.Username==username))
                 return true;
            return false;     
        }
    }
}