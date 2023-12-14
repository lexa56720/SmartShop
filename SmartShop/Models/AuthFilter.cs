using Microsoft.AspNetCore.Routing.Matching;
using SmartShop.Models.DataBase;
using SmartShop.Models.DataBase.Tables;

namespace SmartShop.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AccessAttribute : Attribute
    {
        public Role[] Roles { get; init; }
        public AccessAttribute(params Role[] roles)
        {
            Roles = roles;
        }
    }

    public class AuthFilter
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<AuthFilter> Logger;
        public AuthFilter(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            Next = next;
            Logger = loggerFactory?.CreateLogger<AuthFilter>() ??
            throw new ArgumentNullException(nameof(loggerFactory));
        }
        public async Task InvokeAsync(HttpContext context, ShopContext db)
        {
            if (await IsHaveAccess(context, db))
                await Next(context);
            else
                context.Response.Redirect("/");
        }

        public async Task<bool> IsHaveAccess(HttpContext context, ShopContext db)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
                return false;

            var accessAttribute = endpoint.Metadata.FirstOrDefault(o => o is AccessAttribute) as AccessAttribute;

            if (accessAttribute == default)
                return true;

            var api = await Api.GetApi(context.Request.Cookies, db);

            if (api == null)
                return false;

            if (accessAttribute.Roles.Any(r => r == api.User.Role))
                return true;

            return false;
        }
    }

    public static class AuthFilterExtensions
    {
        public static IApplicationBuilder UseAuthFilter(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuthFilter>();
        }
    }
}
