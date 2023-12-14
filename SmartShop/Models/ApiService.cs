using SmartShop.Models.DataBase.Tables;
using SmartShop.Models.DataBase;
using Microsoft.EntityFrameworkCore;

namespace SmartShop.Models
{
    public class ApiService
    {
        public User? User { get; private set; }
        public ShopContext DB { get; }

        public ApiService(HttpContext context, ShopContext db)
        {
            DB = db;

            if (context.Request.Cookies.TryGetValue("id", out var strId) &&
                context.Request.Cookies.TryGetValue("token", out var token) &&
            int.TryParse(strId, out var id))
            {
                User = Auth(id, token);
            }
        }
        public ApiService(ShopContext db)
        {
            DB = db;
        }
        private User? Auth(int userId, string token)
        {
            if (!IsLegal(userId, token))
                return null;
            return GetUser(userId);
        }

        public async Task<bool> CreateUser(string login, string pass, string name)
        {
            if (await DB.Users.AnyAsync(u => u.Login == login))
                return false;

            var user = new User()
            {
                Login = login,
                Password = pass,
                Name = name,
                Token = GenerateToken(),
                Role = Role.User
            };
            await DB.Users.AddAsync(user);
            await DB.SaveChangesAsync();
            User = user;
            return true;
        }
        public async Task<bool> Login(string login, string pass)
        {
            var user = await GetUser(login);

            if (user == null || user.Password != pass)
                return false;

            user.Token = GenerateToken();
            await DB.SaveChangesAsync();

            User = user;
            return true;
        }

        private bool IsLegal(int userId, string token)
        {
            return DB.Users.Any(u => u.Id == userId && u.Token == token);
        }
        private User GetUser(int userId)
        {
            return DB.Users.FirstOrDefault(u => u.Id == userId);
        }
        private async Task<User?> GetUser(string login)
        {
            return await DB.Users.FirstOrDefaultAsync(u => u.Login == login);
        }
        private string GenerateToken()
        {
            return System.Security.Cryptography.RandomNumberGenerator.GetHexString(64);
        }
    }
}
