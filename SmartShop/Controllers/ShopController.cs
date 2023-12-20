using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.Services;

namespace SmartShop.Controllers
{
    public abstract class ShopController : Controller
    {
        protected readonly ILogger<HomeController> Logger;
        protected readonly ApiService Api;
        protected readonly ShopContext DB;
        public ShopController(ILogger<HomeController> logger, ShopContext context, ApiService api) : base()
        {
            Logger = logger;
            DB = context;
            Api = api;
        }

        public override ViewResult View(string? viewName, object? model)
        {
            ViewBag.User = Api.User;
            return base.View(viewName, model);
        }

        protected async Task<byte[]> ReadFile(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var data = new byte[stream.Length];
            await stream.ReadAsync(data.AsMemory(0));
            return data;
        }
    }
}
