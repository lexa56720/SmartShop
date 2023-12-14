using Microsoft.AspNetCore.Mvc;
using SmartShop.Models;
using SmartShop.Models.DataBase;
using SmartShop.Models.DataBase.Tables;
using System.Diagnostics;

namespace SmartShop.Controllers
{
    public class HomeController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {
        public IActionResult Index()
        {
            if (Api.User != null)
                ViewBag.User = Api.User;

            return View();
        }


        [Access(Role.User)]
        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
