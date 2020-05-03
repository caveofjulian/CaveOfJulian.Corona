using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Roni.Corona.Persistence.Entities;
using Roni.Corona.Shared;

namespace Roni.Corona.Services
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
