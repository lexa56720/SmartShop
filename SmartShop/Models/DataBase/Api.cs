using Microsoft.EntityFrameworkCore;
using SmartShop.Models.DataBase.Tables;

namespace SmartShop.Models.DataBase
{
    public class Api
    {
        public User User { get; }

        private Api(User user)
        {
            User = user;
        }
        public static async Task<User?> CreateUser(string login, string pass, string name, ShopContext db)
        {
            if (await db.Users.AnyAsync(u => u.Login == login))
                return null;

            var user = new User()
            {
                Login = login,
                Password = pass,
                Name = name,
                Token = GenerateToken(),
            };
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }
        public static async Task<Api?> GetApi(int userId, string token, ShopContext db)
        {
            if (!await IsLegal(userId, token, db))
                return null;
            var user = await GetUser(userId, db);
            return new Api(user);
        }
        public static async Task<Api?> GetApi(string login, string pass, ShopContext db)
        {
            if (!await IsLegal(login, pass, db))
                return null;

            var user = await GetUser(login, db);
            user.Token = GenerateToken();
            await db.SaveChangesAsync();

            return new Api(user);
        }
        private static async Task<bool> IsLegal(int userId, string Token, ShopContext db)
        {
            throw new NotImplementedException();
        }
        private static async Task<bool> IsLegal(string login, string pass, ShopContext db)
        {
            throw new NotImplementedException();
        }
        private static async Task<User> GetUser(int userId, ShopContext db)
        {
            throw new NotImplementedException();
        }
        private static async Task<User> GetUser(string login, ShopContext db)
        {
            throw new NotImplementedException();
        }
        private static string GenerateToken()
        {
          return  System.Security.Cryptography.RandomNumberGenerator.GetHexString(64);
        }
    }
}
