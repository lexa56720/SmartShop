using SmartShop.DataBase.Tables;

namespace SmartShop.Models
{
    public class ProfileViewModel
    {
        public Order[] Orders { get; }

        public ProfileViewModel(Order[] orders) 
        { 
            Orders = orders;
        }
    }
}
