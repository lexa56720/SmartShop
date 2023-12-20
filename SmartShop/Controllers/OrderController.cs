using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
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

            ViewBag.CartContent = (await Api.GetSmartphones(productsId))
                                   .Select(s => new KeyValuePair<Smartphone, bool>(s, s.UnitsAvailable > 0))
                                   .ToArray();
            return View();
        }


        [Route("Ordering/")]
        [Access(Role.User)]
        public async Task<IActionResult> Ordering()
        {
            var productsId = GetCartContentIds();

            var order = await Api.CreateOrder(Api.User, productsId);
            ViewBag.Order = order.Code;
            return View();
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
