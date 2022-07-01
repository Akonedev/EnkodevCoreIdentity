using EnkodevCoreIdentity.Data;
using EnkodevCoreIdentity.Interfaces;
using EnkodevCoreIdentity.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EnkodevCoreIdentity.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            _context.Update(user);
            return Save();
        }

       /* public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }*/


        /*public string GetUserId(ClaimsPrincipal clUser)
        {
            return clUser.FindFirst(ClaimTypes.NameIdentifier).Value;
        }*/
    }
}
