using System.ComponentModel.DataAnnotations;

namespace SmartShop.Models.DataBase.Tables
{

    public enum Roles
    {
        User,
        Admin,
    }
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public required Roles Roles { get; set; }
    }
}
