using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
using SmartShop.Models;
using SmartShop.Services;
using SmartShop.Services.Auth;

namespace SmartShop.Controllers
{
    public class CatalogController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {
        public async Task<IActionResult> Index()
        {
            var smartphones = await Api.GetSmartphones(50, 0);     
            return View(new CatalogViewModel(smartphones, smartphones.Length, 0));
        }

        public async Task<IActionResult> Product(int id)
        {
            var smartphone = await Api.GetSmartphone(id);
            if (smartphone == null)
                return BadRequest();
            return View(new ProductViewModel(smartphone));
        }

        [Route("Edit")]
        [Access(Role.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var smartphone = await Api.GetSmartphone(id);
            if (smartphone == null)
                return BadRequest();

            return View(new EditViewModel(smartphone, await Api.GetProducers()));
        }
    }
}
