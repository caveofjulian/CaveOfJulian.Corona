using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Roni.Corona.Persistence.Entities;
using Roni.Corona.Shared;

namespace Roni.Corona.Persistence
{
    public static class PersistenceConfiguration
    {
        public static void ConfigurePersistenceServices(this IServiceCollection collection, string connectionString)
        {
            collection.AddTransient<ICoronaCasesRepository<Cases>, CoronaCasesRepository<Cases>>();
            collection.AddDbContext<RoniContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
