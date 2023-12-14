using Microsoft.AspNetCore.Mvc;
using SmartShop.Models;
using SmartShop.Models.DataBase;
using SmartShop.Models.DataBase.Tables;
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


  
        public async Task<IActionResult> Index()
        {
            var api = await Api.GetApi(HttpContext.Request.Cookies,Context);

            if (api != null)
                ViewBag.User = api.User;
            ViewBag.IsHeaderEnabled = true;
            return View();
        }


        [Access(Role.User)]
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
