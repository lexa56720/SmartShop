using Microsoft.AspNetCore.Mvc;
using SmartShop.Models.DataBase;
using SmartShop.Models.DataBase.Tables;

namespace SmartShop.Controllers
{
    public class ApiController : Controller
    {
        private ILogger<HomeController> Logger { get; }

        public ShopContext Context { get; }

        public ApiController(ILogger<HomeController> logger, ShopContext context)
        {
            Logger = logger;
            Context = context;
        }

        [HttpPost("api/Login")]
        public async Task<IActionResult> Login(string login, string pass)
        {
            var api = await Api.GetApi(login, pass, Context);
            if (api == null)
                return Conflict();
            SetCookie(api.User);
            return Json(api.User);
        }

        [HttpPost("api/Register")]
        public async Task<IActionResult> Register(string name, string login, string pass)
        {
            var user = await Api.CreateUser(login, pass, name, Context);
            if (user == null)
                return Conflict();
            SetCookie(user);
            return Json(user);
        }

        [HttpPost("api/Load")]
        public async Task<IActionResult> Load(int userId, string token)
        {
            var api = await Api.GetApi(userId, token, Context);
            if (api == null)
                return Empty;
            return Json(api);
        }

        private void SetCookie(User user)
        {
            HttpContext.Response.Cookies.Append("name", user.Name);
            HttpContext.Response.Cookies.Append("token", user.Token);
            HttpContext.Response.Cookies.Append("id", user.Id.ToString());
        }
    }
}
