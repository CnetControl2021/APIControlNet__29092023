using Microsoft.AspNetCore.Identity;

namespace APIControlNet.DTOs
{
    public class UserRole
    {
        public IdentityUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
