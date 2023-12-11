using Microsoft.AspNetCore.Mvc;
using SmartShop.Models.DataBase;

namespace SmartShop.Controllers
{
    public class DBController : Controller
    {
        private ILogger<HomeController> Logger { get; }

        public ShopContext Context { get; }

        public DBController(ILogger<HomeController> logger, ShopContext context)
        {
            Logger = logger;
            Context = context;
        }

        [HttpPost("db/Login")]
        public async Task<IActionResult> Login(string login, string pass)
        {
            var api = await Api.GetApi(login, pass, Context);
            if (api == null)
                return Empty;
            return Json(api.User);
        }

        [HttpPost("db/Register")]
        public async Task<IActionResult> Register(string name, string login, string pass)
        {
            var user = await Api.CreateUser(login, pass, name, Context);
            if (user == null)
                return Empty;
            return Json(user);
        }

        [HttpPost("db/Load")]
        public async Task<IActionResult> Load(int userId, string token)
        {
            var api = await Api.GetApi(userId, token, Context);
            if (api == null)
                return Empty;
            return Json(api);
        }
    }
}
