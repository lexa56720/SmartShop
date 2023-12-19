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
        public async Task<IActionResult> Index()
        {
            ViewBag.Smartphones = await Api.GetSmartphones(50, 0);
            return View();
        }


        [Access(Role.User)]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Cart/")]
        public async Task<IActionResult> Cart()
        {
            var cart = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "Cart");
            var productsId = Array.Empty<int>();
            if (!cart.Equals(default(KeyValuePair<string, string>)))
                productsId = cart.Value.Split("|", StringSplitOptions.RemoveEmptyEntries)
                                       .Select(int.Parse)
                                       .ToArray();
            ViewBag.CartContent = await Api.GetSmartphones(productsId);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
