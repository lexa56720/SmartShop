using SmartShop.DataBase.Tables;

namespace SmartShop.Services.Auth
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
}
