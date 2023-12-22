using SmartShop.DataBase.Tables;

namespace SmartShop.Models
{
    public class CatalogViewModel
    {
        public Smartphone[] Smartphones { get; }
        public int Count { get; }
        public int Offset { get; }

        public CatalogViewModel(Smartphone[] smartphones,int count, int offset) 
        {
            Smartphones = smartphones;
            Count = count;
            Offset = offset;
        }
    }
}
