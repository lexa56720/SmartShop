using System.ComponentModel.DataAnnotations;

namespace SmartShop.Models.DataBase
{
    public class Producer
    {
        public int Id { get; set; }


        [Required]
        public required string Name { get; set; }

        public virtual ICollection<Smartphone> Smartphones { get; set; } = null!;
    }
}
