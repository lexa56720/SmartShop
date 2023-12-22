using System.ComponentModel.DataAnnotations;

namespace SmartShop.DataBase.Tables
{
    public class Media
    {
        public int Id { get; set; }

        public required string Url { get; set; }

        public required byte[] Data { get; set; }

        [Required]
        public required int SmartphoneId { get; set; }

        [Required]
        public virtual Smartphone Smartphone { get; set; } = null!;
    }
}
