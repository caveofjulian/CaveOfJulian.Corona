using Corona.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Corona.Persistence
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
