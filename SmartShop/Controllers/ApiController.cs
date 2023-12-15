using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
using SmartShop.Services;
using SmartShop.Services.Auth;

namespace SmartShop.Controllers
{
    public class ApiController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {
        [HttpPost("api/Login")]
        public async Task<IActionResult> Login(string login, string pass)
        {
            await Api.Login(login, pass);
            if (Api.User==null)
                return Conflict();
            SetCookie(Api.User);
            return Ok();
        }

        [HttpPost("api/Register")]
        public async Task<IActionResult> Register(string name, string login, string pass)
        {
            if (!await Api.CreateUser(login, pass, name))
                return Conflict();
            SetCookie(Api.User);
            return Ok();
        }

        [HttpPost("api/AddProduct")]
        [Access(Role.Admin)]
        public async Task<IActionResult> AddProduct(Smartphone smartphone)
        {
            await Api.AddProduct(smartphone);
            return Ok();
        }


        [HttpPost("api/AddProducer")]
        [Access(Role.Admin)]
        public async Task<IActionResult> AddProducer(Producer producer)
        {
            await Api.AddProducer(producer);
            return Ok();
        }
        private void SetCookie(User user)
        {
            HttpContext.Response.Cookies.Append("name", user.Name);
            HttpContext.Response.Cookies.Append("token", user.Token);
            HttpContext.Response.Cookies.Append("id", user.Id.ToString());
        }
    }
}
