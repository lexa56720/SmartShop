using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
using SmartShop.Models.TableEditor;
using SmartShop.Services;
using SmartShop.Services.Auth;
using System.Buffers.Text;
using System.Globalization;
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


        [Access(Role.Admin)]
        [HttpPost("api/AddProduct")]
        public async Task<IActionResult> AddProduct(Smartphone smartphone)
        {
            var form = await HttpContext.Request.ReadFormAsync();

            if (!form.TryGetValue("producerName", out var producer) || !form.TryGetValue("price", out var price))
                return BadRequest();
            smartphone.Price = Convert.ToSingle(price, CultureInfo.InvariantCulture.NumberFormat);
            var filesBytes = new byte[form.Files.Count][];

            await Parallel.ForAsync(0, filesBytes.Length, async (i, c) =>
            {
                filesBytes[i] = await ReadFile(form.Files[i]);
            });

            if (await Api.AddSmartphone(smartphone, producer.ToString(), filesBytes))
                return Ok();
            return BadRequest();
        }

        [Access(Role.Admin)]
        [HttpPost("api/EditProduct")]
        public async Task<IActionResult> EditProduct(Smartphone smartphone)
        {
            var form = await HttpContext.Request.ReadFormAsync();

            if (!form.TryGetValue("producerName", out var producer) || !form.TryGetValue("price", out var price))
                return BadRequest();
            smartphone.Price = Convert.ToSingle(price, CultureInfo.CurrentCulture.NumberFormat);
            if (await Api.EditSmartphone(smartphone, producer.ToString()))
                return Ok();
            return BadRequest();
        }

        [Access(Role.Admin)]
        [HttpPost("api/DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (await Api.DeleteSmartphone(id))
                return Ok();
            return BadRequest();
        }

        [Access(Role.Admin)]
        [HttpPost("api/Load")]
        public IActionResult Load(string? table)
        {
            if (table == null)
                return BadRequest();

            var tableEntity = GetTable(table);
            if (tableEntity != null)
                return PartialView("TableEditor", tableEntity);

            return BadRequest();
        }

        [Access(Role.Admin)]
        [HttpPost("api/Save")]
        public async Task<IActionResult> Save(string? table)
        {
            if (table == null)
                return BadRequest();

            var body = await ReadRequestBody(Request);
            var args = body.Split(';');

            var tableEntity = GetTable(table);
            if (tableEntity != null && (await tableEntity.UpdateRow(args)))
                return PartialView("TableEditor", tableEntity);

            return BadRequest();
        }

        [Access(Role.Admin)]
        [HttpPost("api/Delete")]
        public async Task<IActionResult> Delete(string? table)
        {
            if (table == null)
                return BadRequest();

            var body = await ReadRequestBody(Request);
            var args = body.Split(';');

            var tableEntity = GetTable(table);
            if (tableEntity != null && (await tableEntity.DeleteRow(args)))
                return PartialView("TableEditor", tableEntity);

            return BadRequest();
        }

        [Access(Role.Admin)]
        [HttpPost("api/Add")]
        public async Task<IActionResult> Add(string? table)
        {
            if (table == null)
                return BadRequest();

            var body = await ReadRequestBody(Request);
            var args = body.Split(';');

            var tableEntity = GetTable(table);
            if (tableEntity != null && (await tableEntity.AddRow(args)))
                return PartialView("TableEditor", tableEntity);

            return BadRequest();
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            var sr = new StreamReader(request.Body);
            return await sr.ReadToEndAsync();
        }

        private ITableWorker? GetTable(string tableName)
        {
            var tableEntity = DB.Model.GetEntityTypes()
                  .Where(t => t.GetTableName() == tableName)
                  .First();

            Type type = tableEntity.ClrType;
            var workerType = typeof(TableWorker<>).MakeGenericType(type);
            var worker = Activator.CreateInstance(workerType, tableEntity, context);
            return worker as ITableWorker;
        }
        private void SetCookie(User user)
        {
            HttpContext.Response.Cookies.Append("name", user.Name);
            HttpContext.Response.Cookies.Append("token", user.Token);
            HttpContext.Response.Cookies.Append("id", user.Id.ToString());
        }
    }
}
