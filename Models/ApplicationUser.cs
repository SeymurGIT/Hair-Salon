using Microsoft.AspNetCore.Identity;
#nullable enable

namespace HairSalon.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? UserImageLink { get; set; }
    }
}
