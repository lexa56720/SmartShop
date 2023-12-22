using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.Services;

namespace SmartShop.Controllers
{
    public class ResourceController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {

        [HttpGet("/images/{id}")]
        public async Task<IActionResult> GetMedia(string id)
        {
            var extension = id[(id.LastIndexOf('.') + 1)..];
            var media = await Api.GetMedia("/images/" + id[0..id.LastIndexOf('.')]);
            return File(media, $"image/{extension}");
        }

    }
}
