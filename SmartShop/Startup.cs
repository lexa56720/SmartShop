using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using SmartShop.DataBase;
using SmartShop.Services;
using SmartShop.Services.Auth;

namespace SmartShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var s = Configuration.GetConnectionString("ShopConnection");
            services.AddDbContext<ShopContext>(options => options.UseNpgsql(s));
            services.AddHttpContextAccessor();
            services.AddScoped(provider =>
            {
                var context = provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var db = provider.GetRequiredService<ShopContext>();
                if (context == null)
                    return new ApiService(db);
                return new ApiService(context, db);
            });
            services.AddControllersWithViews();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthFilter();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Catalog}/{action=Index}/{id?}");
            });

        }
    }
}
