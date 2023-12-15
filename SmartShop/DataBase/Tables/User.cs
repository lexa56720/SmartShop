using System.ComponentModel.DataAnnotations;

namespace SmartShop.DataBase.Tables
{
    public enum Role
    {
        User = 0,
        Admin = 1,
    }
    public class User
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Login { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string Token { get; set; } = string.Empty;

        [Required]
        public required Role Role { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = null!;
    }
}
