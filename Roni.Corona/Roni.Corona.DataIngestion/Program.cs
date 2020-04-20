using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Roni.Corona.DataIngestion.Integrations;
using Roni.Corona.Persistence;
using Roni.Corona.Services;

namespace Roni.Corona.DataIngestion
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new Logger<Ingester>(new LoggerFactory());
            var connectionString = "Server=(localdb)\\MSSQLLocalDb;Database=RoniDb;Trusted_Connection=True;";

            var integration = new CsseIntegration(new HttpClient());
            var optionsBuilder = new DbContextOptionsBuilder<RoniContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var context = new RoniContext(optionsBuilder.Options);
            var service = new CoronaService(new CoronaCasesRepository<Roni.Corona.Persistence.Entities.Cases>(context));
            var ingester = new Ingester(integration, service, logger);

            await ingester.CheckForUpdates();
        }
    }
}
