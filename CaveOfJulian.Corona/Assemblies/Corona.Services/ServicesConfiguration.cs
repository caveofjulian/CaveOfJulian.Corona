using AutoMapper;
using Corona.Persistence.Entities;
using Corona.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Corona.Services
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this IServiceCollection collection)
        {
            var config = new MapperConfiguration(config => config.CreateMap<Cases, CaseReport>());
            collection.AddSingleton<IMapper>(new Mapper(config));
        }
    }
}
