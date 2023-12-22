using SmartShop.DataBase.Tables;

namespace SmartShop.Models
{
    public class CartViewModel
    {
        public KeyValuePair<Smartphone, bool>[] CartContent { get; }

        public CartViewModel(Smartphone[] smartphones)
        {
            CartContent = smartphones.Select(s => new KeyValuePair<Smartphone, bool>(s, s.UnitsAvailable > 0))
                                     .ToArray();
        }
    }
}
