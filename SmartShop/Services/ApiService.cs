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
        private ShopContext DB { get; }

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
                Token = GenerateToken(64),
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

            user.Token = GenerateToken(64);
            await DB.SaveChangesAsync();

            User = user;
            return true;
        }

        public async Task<Producer[]> GetProducers()
        {
            return await DB.Producers.ToArrayAsync();
        }
        public async Task<Producer[]> GetProducers(int[] ids)
        {
            var producers = await DB.Producers
                                    .Where(s => ids.Contains(s.Id))
                                    .ToArrayAsync();

            var result = new List<Producer>();
            for (int i = 0; i < ids.Length; i++)
            {
                var smartphone = producers.FirstOrDefault(s => s.Id == ids[i]);
                if (smartphone != null)
                    result.Add(smartphone);
            }

            return result.ToArray();
        }
        public async Task<bool> DeleteSmartphone(int id)
        {
            try
            {
                await DB.Smartphones.Where(s => s.Id == id).ExecuteDeleteAsync();
                await DB.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public async Task<Order[]> GetOrders(int userId)
        {
            var orders = DB.Users.Include(u => u.Orders)
                                 .ThenInclude(o=>o.Smartphones)
                                 .Where(u => u.Id == userId)
                                 .SelectMany(u => u.Orders);

            return await orders.ToArrayAsync();
        }
        public async Task<bool> EditSmartphone(Smartphone smartphone, string producerName)
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
            var dbPhone = await DB.Smartphones.FirstOrDefaultAsync(s => s.Id == smartphone.Id);

            if (dbPhone == null)
                return false;

            dbPhone.Name = smartphone.Name;
            dbPhone.Description = smartphone.Description;
            dbPhone.Price = smartphone.Price;
            dbPhone.Producer = producer;
            dbPhone.MegaPixels = smartphone.MegaPixels;
            dbPhone.RamSize = smartphone.RamSize;
            dbPhone.MemorySize = smartphone.MemorySize;
            dbPhone.ReleaseDate = smartphone.ReleaseDate;
            dbPhone.UnitsAvailable = smartphone.UnitsAvailable;

            await DB.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddSmartphone(Smartphone smartphone, string producerName, byte[][] images)
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

            return true;
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

        public async Task<Smartphone[]> GetSmartphones()
        {
            return await DB.Smartphones.Include(s => s.Medias)
                                       .ToArrayAsync();
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
        public async Task<Smartphone?> GetSmartphone(int id)
        {
            return await DB.Smartphones
                .Include(s => s.Producer)
                .Include(s => s.Medias)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Order?> CreateOrder(User user, int[] smartphoneIds)
        {
            var orderContent = new List<Smartphone>();
            for (int i = 0; i < smartphoneIds.Length; i++)
            {
                var smartphone = DB.Smartphones.Find(smartphoneIds[i]);
                if (smartphone == null || smartphone.UnitsAvailable < 1)
                    continue;
                smartphone.UnitsAvailable--;
                orderContent.Add(smartphone);
            }
            var order = new Order()
            {
                User = user,
                Code = GenerateToken(6),
                OrderDate = DateTime.UtcNow,
                Price = orderContent.Sum(c => c.Price),
                Smartphones = orderContent,
                Status = OrderStatus.Ordered,

            };
            await DB.Orders.AddAsync(order);
            await DB.SaveChangesAsync(); 
            return order;
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

        private string GenerateToken(int size)
        {
            return RandomNumberGenerator.GetHexString(size);
        }
        private string GenerateImageUrl()
        {
            return $"/images/{RandomNumberGenerator.GetHexString(16, true)}.png";
        }
    }
}
