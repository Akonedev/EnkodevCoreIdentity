using EnkodevCoreIdentity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace EnkodevCoreIdentity.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);

        Task<List<string>> GetUserRoles(AppUser user);

        //async Task<IEnumerable<string>> GetUserRoles(string id)
        // Task<AppUser> GetUserId(object ClaimsPrincipal user);

        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
