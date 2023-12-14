using Microsoft.AspNetCore.Mvc;
using SmartShop.Models;
using SmartShop.Models.DataBase;

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