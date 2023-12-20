using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.Services;

namespace SmartShop.Controllers
{
    public class CatalogController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Smartphones = await Api.GetSmartphones(50, 0);
            return View();
        }

        public async Task<IActionResult> Product(int id)
        {
            var smartphone = await Api.GetSmartphone(id);
            if (smartphone == null)
                return BadRequest();
            ViewBag.Smartphone=smartphone;
            return View();
        }

        [Route("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var smartphone = await Api.GetSmartphone(id);
            if (smartphone == null)
                return BadRequest();

            ViewBag.Smartphone = smartphone;
            ViewBag.Producers =await Api.GetProducers();
            return View();
        }
    }
}
