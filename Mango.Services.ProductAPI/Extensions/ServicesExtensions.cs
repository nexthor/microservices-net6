using Mango.Services.ProductAPI.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
