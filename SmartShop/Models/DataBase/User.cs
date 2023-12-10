using System.ComponentModel.DataAnnotations;

namespace SmartShop.Models.DataBase
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Login { get; set; }

        [Required]
        public required string Password { get; set; }

        public string Token { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = null!;
    }
}
