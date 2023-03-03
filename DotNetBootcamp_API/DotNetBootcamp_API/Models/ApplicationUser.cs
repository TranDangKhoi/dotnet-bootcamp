using Microsoft.AspNetCore.Identity;

namespace DotNetBootcamp_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
