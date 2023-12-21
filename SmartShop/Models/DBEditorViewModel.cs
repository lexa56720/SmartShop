using Microsoft.EntityFrameworkCore;
using SmartShop.DataBase.Tables;

namespace SmartShop.Models
{
    public class AdminPanelViewModel
    {

        public string[] Tables { get; set; }
        public Producer[] Producers { get; }

        public AdminPanelViewModel(DbContext context,Producer[] producers)
        {
            Tables = context.Model.GetEntityTypes()
                                  .Select(t => t.GetTableName())
                                  .Where(t => t != null)
                                  .Distinct()
                                  .ToArray();
            Producers = producers;
        }
    }


}
