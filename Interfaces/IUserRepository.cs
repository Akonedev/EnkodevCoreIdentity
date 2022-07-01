using EnkodevCoreIdentity.Models;
using System.Security.Claims;

namespace EnkodevCoreIdentity.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);

        //string GetUserId(ClaimsPrincipal user);
        // Task<AppUser> GetUserId(object ClaimsPrincipal user);

        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
