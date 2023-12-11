namespace SmartShop.Models.DataBase.Tables
{
    public enum OrderStatus
    {
        Ordered,
        Shipped,
        Arrived
    }
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int Price { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public virtual ICollection<Smartphone> Smartphones { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
