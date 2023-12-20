using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.Services;

namespace SmartShop.Controllers
{
    public class AuthController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {
        public IActionResult Login()
        {
            ViewBag.IsHeaderDisabled = true; 
            return View();
        }

        public IActionResult Register()
        {
            ViewBag.IsHeaderDisabled = true;
            return View();
        }

    }
}