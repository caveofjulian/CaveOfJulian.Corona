using Microsoft.EntityFrameworkCore;
using Roni.Corona.Persistence.Entities;

namespace Roni.Corona.Persistence
{
    public class RoniContext : DbContext
    {
        public RoniContext(DbContextOptions options)  : base(options){ }

        public DbSet<Cases> Cases { get; set; }
    }
}
