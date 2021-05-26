using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Employee Employee { get; set; }
    }
}