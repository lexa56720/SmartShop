using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.Services;

namespace SmartShop.Controllers
{
    public class CatalogController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Product(int id)
        {
            return View();
        }
    }
}
