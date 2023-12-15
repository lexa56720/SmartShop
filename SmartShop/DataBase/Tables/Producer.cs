using System.ComponentModel.DataAnnotations;

namespace SmartShop.DataBase.Tables
{
    public class Producer
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }
        public virtual ICollection<Smartphone> Smartphones { get; set; } = null!;
    }
}
