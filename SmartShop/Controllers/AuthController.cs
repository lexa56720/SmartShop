using Microsoft.AspNetCore.Mvc;
using SmartShop.Models.DataBase;

namespace SmartShop.Controllers
{
    public class AuthController:Controller
    {
        public AuthController(ILogger<HomeController> logger, ShopContext context)
        {
        }
        public IActionResult Login()
        {
            ViewBag.IsHeaderEnabled = false;
            return View();
        }

        public IActionResult Register()
        {
            ViewBag.IsHeaderEnabled = false;
            return View();
        }
    }
}
