using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Roni.Corona.Persistence.Entities;

namespace Roni.Corona.Persistence
{
    public static class ServicesConfiguration
    {
        public static void ConfigurePersistenceServices(this IServiceCollection collection, string connectionString)
        {
            collection.AddTransient<ICoronaCasesRepository<Cases>, CoronaCasesRepository<Cases>>();
            collection.AddDbContext<RoniContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
