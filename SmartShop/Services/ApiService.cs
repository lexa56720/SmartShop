using Microsoft.EntityFrameworkCore;
using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
using System.Security.Cryptography;

namespace SmartShop.Services
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

        public async Task<Producer[]> GetProducers()
        {
            return await DB.Producers.ToArrayAsync();
        }

        public async Task AddProducer(Producer producer)
        {
            await DB.Producers.AddAsync(producer);
            await DB.SaveChangesAsync();
        }
        public async Task AddProduct(Smartphone smartphone)
        {
            smartphone.ReleaseDate = smartphone.ReleaseDate.ToUniversalTime();
            await DB.Smartphones.AddAsync(smartphone);
            await DB.SaveChangesAsync();
        }
        public async Task AddMedia(byte[] bytes,int smartphoneId)
        {
            var media = new Media()
            {
                Data = bytes,
                SmartphoneId = smartphoneId,
                Url = GenerateImageUrl(),
            };
            await DB.Medias.AddAsync(media);
            await DB.SaveChangesAsync();
        }
        private bool IsLegal(int userId, string token)
        {
            return DB.Users.Any(u => u.Id == userId && u.Token == token);
        }
        private User? GetUser(int userId)
        {
            return DB.Users.FirstOrDefault(u => u.Id == userId);
        }
        private async Task<User?> GetUser(string login)
        {
            return await DB.Users.FirstOrDefaultAsync(u => u.Login == login);
        }
        private string GenerateToken()
        {
            return RandomNumberGenerator.GetHexString(64);
        }

        private string GenerateImageUrl()
        {
            return $"images/{RandomNumberGenerator.GetHexString(64)}.png";
        }
    }
}
