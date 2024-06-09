using Microsoft.AspNetCore.Identity;

namespace api_with_auth.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
