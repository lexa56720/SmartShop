using SmartShop.DataBase.Tables;

namespace SmartShop.Models
{
    public class EditViewModel
    {
        public Smartphone Smartphone { get;  }

        public Producer[] Producers { get;  }
        public EditViewModel(Smartphone smartphone, Producer[] producers)
        {
            Smartphone = smartphone;
            Producers = producers;
        }
    }
}
