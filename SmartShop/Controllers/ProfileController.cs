using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
using SmartShop.Services;

namespace SmartShop.Controllers
{
    public class ProfileController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {
        public IActionResult Index()
        {
            if (Api.User != null && Api.User.Role == Role.Admin)
                return View("AdminPanel");

            return View();
        }
    }
}
