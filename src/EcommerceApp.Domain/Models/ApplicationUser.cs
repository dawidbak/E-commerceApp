using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Employee Employee { get; set; }

        public virtual Customer Customer { get; set; }
    }
}