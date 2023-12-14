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
        public AuthFilter(RequestDelegate next)
        {
            Next = next;
        }
        public async Task InvokeAsync(HttpContext context, ApiService api)
        {
            if (IsHaveAccess(context, api))
                await Next(context);
            else
                context.Response.Redirect("/");
        }

        public bool IsHaveAccess(HttpContext context, ApiService api)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
                return false;

            var accessAttribute = endpoint.Metadata.FirstOrDefault(o => o is AccessAttribute) as AccessAttribute;

            return (api.User != null || accessAttribute == null) &&
                   (accessAttribute == null || accessAttribute.Roles.Any(r => r <= api.User.Role));
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
