using Microsoft.AspNetCore.Mvc;
using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
using SmartShop.Models;
using SmartShop.Models.TableEditor;
using SmartShop.Services;
using SmartShop.Services.Auth;

namespace SmartShop.Controllers
{
    public class ProfileController(ILogger<HomeController> logger, ShopContext context, ApiService api) : ShopController(logger, context, api)
    {
        [Access(Role.User)]
        public async Task<IActionResult> Index()
        {
            if (Api.User != null && Api.User.Role == Role.Admin)
                return await AdminPanel();

            return View(new ProfileViewModel(await Api.GetOrders(Api.User.Id)));
        }

        [Access(Role.Admin)]
        public async Task<IActionResult> AdminPanel()
        {
            return View("AdminPanel", new AdminPanelViewModel(context, await Api.GetProducers()));
        }
    }
}
