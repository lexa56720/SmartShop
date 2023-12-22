using SmartShop.DataBase.Tables;

namespace SmartShop.Models
{
    public class OrderingViewModel
    {
        public Order Order { get; }

        public OrderingViewModel(Order order)
        {
            Order = order;
        }
    }
}
