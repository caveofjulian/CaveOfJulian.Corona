using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Roni.Corona.DataIngestion.Integrations;
using Roni.Corona.Persistence;
using Roni.Corona.Persistence.Entities;
using Roni.Corona.Services;
using Roni.Corona.Shared;

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
            
            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());
            var mapper = new Mapper(config);
            
            var service = new CoronaService(new CoronaCasesRepository<Roni.Corona.Persistence.Entities.Cases>(context), mapper);
            var ingester = new Ingester(integration, service, logger, mapper);

            await ingester.CheckForUpdates();
        }
    }
}
