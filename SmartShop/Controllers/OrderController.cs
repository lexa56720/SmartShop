using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
using SmartShop.Models;
using SmartShop.Services;
using SmartShop.Services.Auth;

namespace SmartShop.Controllers
{
    public class OrderController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {
        [Route("Cart/")]
        public async Task<IActionResult> Cart()
        {
            var productsId = GetCartContentIds();

            return View(new CartViewModel(await Api.GetSmartphones(productsId)));
        }


        [Route("Ordering/")]
        [Access(Role.User)]
        public async Task<IActionResult> Ordering()
        {
            var productsId = GetCartContentIds();

            return View(await Api.CreateOrder(Api.User, productsId));
        }

        private int[] GetCartContentIds()
        {
            var cart = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "Cart");
            var productsId = Array.Empty<int>();
            if (!cart.Equals(default(KeyValuePair<string, string>)))
                productsId = cart.Value.Split("|", StringSplitOptions.RemoveEmptyEntries)
                                       .Select(int.Parse)
                                       .ToArray();

            return productsId;
        }
    }
}
