using Microsoft.AspNetCore.Mvc;
using SmartShop.Models;
using SmartShop.Models.DataBase;
using System.Diagnostics;

namespace SmartShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ShopContext Context { get; }

        public HomeController(ILogger<HomeController> logger, ShopContext context)
        {
            _logger = logger;
            Context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

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
