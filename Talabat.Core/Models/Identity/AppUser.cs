using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string? DisplayName { get; set; }

        // Make first and last name in the address class as maybe in the future we will have multiple addresses
        public Address? Address { get; set; } // Navigational Property [ONE] 
    }
}
