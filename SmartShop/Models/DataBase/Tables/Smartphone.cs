using System.ComponentModel.DataAnnotations;

namespace SmartShop.Models.DataBase.Tables
{
    public class Smartphone
    {
        public int Id { get; set; }

        public int ProducerId { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        public required int Price { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required int MemorySize { get; set; }

        [Required]
        public required int RamSize { get; set; }

        [Required]
        public required int MegaPixels { get; set; }


        public virtual Producer Producer { get; set; } = null!;
    }
}
