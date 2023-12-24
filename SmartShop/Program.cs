using SmartShop.DataBase;
using SmartShop.DataBase.Tables;
using SmartShop.Services;

namespace SmartShop
{
    public class Program
    {
        static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();
         
        }
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
        }
    }
}