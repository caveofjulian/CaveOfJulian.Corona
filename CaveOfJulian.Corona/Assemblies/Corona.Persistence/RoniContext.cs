using Corona.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Corona.Persistence
{
    public class RoniContext : DbContext
    {
        public RoniContext(DbContextOptions options)  : base(options){ }

        public DbSet<Cases> Cases { get; set; }
    }
}
