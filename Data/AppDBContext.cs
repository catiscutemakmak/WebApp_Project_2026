using Microsoft.EntityFrameworkCore;
using hateekub.Models;

namespace hateekub.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

    }
}