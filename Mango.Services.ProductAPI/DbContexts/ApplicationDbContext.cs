using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }


    }
}
