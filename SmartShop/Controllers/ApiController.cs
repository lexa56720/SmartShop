using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
using SmartShop.Services;
using SmartShop.Services.Auth;
using System.Buffers.Text;
using System.Net;
using System.Net.Http;

namespace SmartShop.Controllers
{
    public class ApiController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {
        [HttpPost("api/Login")]
        public async Task<IActionResult> Login(string login, string pass)
        {
            await Api.Login(login, pass);
            if (Api.User == null)
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
            var form = await HttpContext.Request.ReadFormAsync();

            if (!form.TryGetValue("producerName", out var producer))
                return BadRequest();


            var filesBytes = new byte[form.Files.Count][];


            await Parallel.ForAsync(0, filesBytes.Length, async (i, c) =>
            {
                filesBytes[i] = await ReadFile(form.Files[i]);
            });


            await Api.AddProduct(smartphone, producer.ToString(), filesBytes);
            return Ok();
        }


        [HttpGet("/images/{id}")]
        public async Task<IActionResult> GetMedia(string id)
        {
            var extension = id[(id.LastIndexOf('.') + 1)..];
            var media = await Api.GetMedia("images/" + id);
            return File(media, $"image/{extension}");
        }


        private void SetCookie(User user)
        {
            HttpContext.Response.Cookies.Append("name", user.Name);
            HttpContext.Response.Cookies.Append("token", user.Token);
            HttpContext.Response.Cookies.Append("id", user.Id.ToString());
        }
        private async Task<byte[]> ReadFile(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var data = new byte[stream.Length];
            await stream.ReadAsync(data.AsMemory(0));
            return data;
        }
    }
}
