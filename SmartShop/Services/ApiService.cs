using Microsoft.EntityFrameworkCore;
using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

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

        public async Task AddProduct(Smartphone smartphone, string producerName, byte[][] images)
        {
            var producer = await DB.Producers.FirstOrDefaultAsync(p => p.Name == producerName);
            if (producer == null)
            {
                producer = new Producer() { Name = producerName };
                await DB.Producers.AddAsync(producer);
                await DB.SaveChangesAsync();
            }
            smartphone.Producer = producer;
            smartphone.ReleaseDate = smartphone.ReleaseDate.ToUniversalTime();
            await DB.Smartphones.AddAsync(smartphone);
            await DB.SaveChangesAsync();

            foreach (var image in images)
                await AddMedia(image, smartphone.Id);
        }
        public async Task AddMedia(byte[] bytes, int smartphoneId)
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

        public async Task<byte[]> GetMedia(string url)
        {
            var data = await DB.Medias.FirstOrDefaultAsync(m => m.Url == url);
            if (data == null)
                return Array.Empty<byte>();
            return data.Data;
        }

        public async Task<Smartphone[]> GetSmartphones(int count, int offset)
        {
            return await DB.Smartphones.Skip(offset)
                                       .Take(count)
                                       .Include(s => s.Medias)
                                       .ToArrayAsync();
        }
        public async Task<Smartphone?> GetSmartphone(int id)
        {
            return await DB.Smartphones
                .Include(s => s.Producer)
                .Include(s => s.Medias)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Smartphone[]> GetSmartphones(int[] ids)
        {
            var smartphones = await DB.Smartphones
                .Include(s => s.Producer)
                .Include(s => s.Medias)
                .Where(s => ids.Contains(s.Id)).ToArrayAsync();

            var result = new List<Smartphone>();
            for (int i = 0; i < ids.Length; i++)
            {
                var smartphone = smartphones.FirstOrDefault(s => s.Id == ids[i]);
                if (smartphone != null)
                    result.Add(smartphone);
            }

            return result.ToArray();
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
            return $"/images/{RandomNumberGenerator.GetHexString(16, true)}.png";
        }
    }
}
