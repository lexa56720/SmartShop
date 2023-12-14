using Microsoft.AspNetCore.Mvc;
using SmartShop.Models.DataBase;
using SmartShop.Models;

namespace SmartShop.Controllers
{
    public abstract class ShopController:Controller
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


        public override ViewResult View()
        {
            ViewBag.User = Api.User;
            return base.View();
        }

    }
}
