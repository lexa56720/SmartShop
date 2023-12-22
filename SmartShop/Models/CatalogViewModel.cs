using SmartShop.DataBase.Tables;

namespace SmartShop.Models
{
    public class CatalogViewModel
    {
        public Smartphone[] Smartphones { get;  }
        public Producer[] Producers { get;  }
        public Producer[] ProducerFilter { get; }

        public int Offset { get; }
        public int TotalCount { get; }

        public int CurrentPage { get; }

        public bool IsHavePrevious { get; }
        public bool IsHaveNext { get; }

        public CatalogViewModel(Producer[] producerFilter,Producer[] producers,Smartphone[] smartphones, int itemsPerPage, int offset, int totalCount)
        {
            ProducerFilter = producerFilter;
            Producers = producers;
            Smartphones = smartphones;
            Offset = offset;
            TotalCount = totalCount;

            CurrentPage = (int)MathF.Floor(offset / itemsPerPage);

            IsHavePrevious = offset - itemsPerPage >= 0;
            IsHaveNext = offset + itemsPerPage <= totalCount;
        }
    }
}
