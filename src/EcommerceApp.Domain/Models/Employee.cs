namespace EcommerceApp.Domain.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; }
    }
}