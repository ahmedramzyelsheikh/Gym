using Microsoft.AspNetCore.Identity;

namespace GYM.DAL.Data.Models
{
    // Staff account used to log in to the system (ASP.NET Core Identity).
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
