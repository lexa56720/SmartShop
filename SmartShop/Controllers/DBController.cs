using SmartShop.Models.DataBase;

namespace SmartShop.Controllers
{
    public class DBController
    {
        private ILogger<HomeController> Logger { get; }

        public ShopContext Context { get; }

        public DBController(ILogger<HomeController> logger, ShopContext context)
        {
            Logger = logger;
            Context = context;
        }
    }
}
