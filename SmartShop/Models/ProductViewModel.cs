using SmartShop.DataBase.Tables;

namespace SmartShop.Models
{
    public class ProductViewModel
    {
        public Smartphone Smartphone { get; }

        public ProductViewModel(Smartphone smartphone)
        {
            Smartphone = smartphone;
        }
    }
}
