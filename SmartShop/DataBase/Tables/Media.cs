namespace SmartShop.DataBase.Tables
{
    public class Media
    {
        public int Id { get; set; }

        public required string Url { get; set; }

        public required byte[] Data { get; set; }

        public required int SmartphoneId { get; set; }
        public virtual Smartphone Smartphone { get; set; } = null!;
    }
}
