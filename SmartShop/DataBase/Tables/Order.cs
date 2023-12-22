using System.ComponentModel.DataAnnotations;

namespace SmartShop.DataBase.Tables
{
    public enum OrderStatus
    {
        Ordered=0,
        Shipping=1,
        Arrived=2,
        Completed=3,
    }
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public required float Price { get; set; }
        [Required]
        public required string Code { get; set; }
        [Required]
        public required DateTime OrderDate { get; set; }
        [Required]
        public required OrderStatus Status { get; set; }
        [Required]
        public virtual ICollection<Smartphone> Smartphones { get; set; } = null!;

        [Required]
        public virtual User User { get; set; } = null!;
    }
}
