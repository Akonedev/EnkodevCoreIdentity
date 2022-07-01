using EnkodevCoreIdentity.Data;
using EnkodevCoreIdentity.Interfaces;
using EnkodevCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EnkodevCoreIdentity.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public UserRepository(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public bool Add(AppUser user)
        {
            _context.Add(user);
            return Save();
        }

        public bool Delete(AppUser user)
        {
            _context.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<string>> GetUserRoles(AppUser user)
        {
            //var userList = _context.Users.ToList();
            /*var roleList = _context.UserRoles.ToList();
            var roles =  _context.Roles.ToList();
            //set user to none to not make ui look terrible

            var user = await _context.Users.FindAsync(id);
            var userRoles = _context.UserRoles.ToList();
            foreach (var rl in userRoles)
            {
                var role = userRoles.FirstOrDefault(u => u.UserId == user.Id);
                if (role == null)
                {
                    user.Role = "None";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == role.RoleId).Name;
                    user.RoleId = roles.FirstOrDefault(u => u.Id == role.RoleId).Id;
                    user.RoleList.Add(user.Role);
                }
            *//*}*//*
            var userRole = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();

            var role = userRole.Select(u => u.UserId == id).ToList();
            List<string> userRoles = (List<string>)await _userManager.GetRolesAsync(user);*/

            return (List<string>)await _userManager.GetRolesAsync(user);
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

       /* Task<IEnumerable<string>> IUserRepository.GetUserRoles()
        {
            throw new NotImplementedException();
        }*/
    }
}
