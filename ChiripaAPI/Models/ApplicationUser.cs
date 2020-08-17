using Microsoft.AspNetCore.Identity;

namespace ChiripaAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nombre { get; set; }
    }
}