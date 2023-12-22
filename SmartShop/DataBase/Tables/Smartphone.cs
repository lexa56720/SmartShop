using System.ComponentModel.DataAnnotations;

namespace SmartShop.DataBase.Tables
{
    public class Smartphone
    {
        public int Id { get; set; }

        public int ProducerId { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        public required int UnitsAvailable { get; set; }

        [Required]
        public required float Price { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required int MemorySize { get; set; }

        [Required]
        public required int RamSize { get; set; }

        [Required]
        public required int MegaPixels { get; set; }

        [Required]
        public required string Description { get; set; }

        public virtual IList<Media> Medias { get; set; } = null!;

        public virtual Producer Producer { get; set; } = null!;
    }
}
