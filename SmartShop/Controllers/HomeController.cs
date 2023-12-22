using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
using SmartShop.Models;
using SmartShop.Services;
using SmartShop.Services.Auth;
using System.Diagnostics;

namespace SmartShop.Controllers
{
    public class HomeController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
