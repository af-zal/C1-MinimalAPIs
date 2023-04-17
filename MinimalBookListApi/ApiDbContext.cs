using Microsoft.EntityFrameworkCore;

namespace MinimalBookListApi
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Book> Books => Set<Book>();
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }

        
    }
}
