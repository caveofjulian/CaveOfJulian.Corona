using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Corona.DataIngestion.Integrations;
using Corona.Persistence;
using Corona.Persistence.Entities;
using Corona.Services;
using Corona.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Roni.Corona.Persistence;

namespace Corona.DataIngestion
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = new Logger<Ingester>(new LoggerFactory());
            var connectionString = "Server=(localdb)\\MSSQLLocalDb;Database=RoniCoronaDb;Trusted_Connection=True;";

            var integration = new CsseIntegration(new HttpClient());
            var optionsBuilder = new DbContextOptionsBuilder<RoniContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var context = new RoniContext(optionsBuilder.Options);
            
            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());
            var mapper = new Mapper(config);
            
            var service = new CoronaService(new CoronaCasesRepository<Cases>(context), mapper);
            var ingester = new Ingester(integration, service, logger);

            await ingester.CheckForUpdates();
        }
    }
}
