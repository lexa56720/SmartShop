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
        private readonly int ProductsPerPage = 9;

        public async Task<IActionResult> Index(int page, int[] producerFilters)
        {
            var smartphones = await Api.GetSmartphones();
            var producers = await Api.GetProducers();
            var producerFilter = await Api.GetProducers(producerFilters);

            if (producerFilter.Length > 0)
                smartphones = smartphones.Where(s => producerFilter.Contains(s.Producer)).ToArray();


            var totalCount = smartphones.Count();
            if (page > totalCount / ProductsPerPage)
                return BadRequest();

            smartphones = smartphones.Skip(page * ProductsPerPage)
                                     .Take(ProductsPerPage).ToArray();

            return View(new CatalogViewModel(producerFilter, producers, smartphones, ProductsPerPage,
                                             page * ProductsPerPage, totalCount));
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
