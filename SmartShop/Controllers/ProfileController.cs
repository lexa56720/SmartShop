using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
using SmartShop.Services;

namespace SmartShop.Controllers
{
    public class ProfileController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {
        public async Task<IActionResult> Index()
        {
            if (Api.User != null && Api.User.Role == Role.Admin)
                return await AdminPanel();

            ViewBag.Orders=Api.Get
            return View();
        }

        public async Task<IActionResult> AdminPanel()
        {
            ViewBag.Producers =await Api.GetProducers();

            return View("AdminPanel");
        }
    }
}
