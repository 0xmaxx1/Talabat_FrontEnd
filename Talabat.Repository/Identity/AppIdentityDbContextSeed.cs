using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                AppUser user = new AppUser()
                {
                    DisplayName = "Ahmed Ali",
                    Email = "ahmed.ali@gmail.com",
                    UserName = "ahmed.ali",
                    PhoneNumber = "01122334455"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

        }
    }
}
