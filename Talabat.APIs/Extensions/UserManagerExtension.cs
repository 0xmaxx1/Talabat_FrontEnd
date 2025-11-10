using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Talabat.Core.Models.Identity;

namespace Talabat.APIs.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser?> GetUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);

            AppUser? user = await userManager.Users.Include(U => U.Address).SingleOrDefaultAsync(U => U.Email == email);


            return user;
        }
    }
}
